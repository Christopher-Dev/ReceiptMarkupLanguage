// Ensure the Monaco editors object is initialized
window.monacoEditors = window.monacoEditors || {};
console.log("Monaco editors object initialized.");

// Define tag attributes with additional metadata for self-closing and closable tags
const tagDefinitions = {
    'Receipt': { attributes: [], selfClosing: true, canClose: false },
    'Body': { attributes: ['Width', 'Background'], selfClosing: false, canClose: false },
    'DataContext': { attributes: [], selfClosing: true, canClose: false },
    'Run': { attributes: ['FontWeight', 'FontSize', 'FontFamily', 'Content'], selfClosing: true, canClose: false },
    'DataSource': { attributes: ['Name'], selfClosing: false, canClose: false },
    'Resources': { attributes: [], selfClosing: false, canClose: false },
    'Repeater': { attributes: ['DataSource'], selfClosing: false, canClose: true },
    'Pair': { attributes: ['Key', 'Value'], selfClosing: true, canClose: false },
    'Border': { attributes: ['Width'], selfClosing: true, canClose: false }, // Updated
    'Row': { attributes: ['Margin'], selfClosing: false, canClose: true },  // Updated
    'Image': { attributes: ['VerticalAlignment', 'Width', 'HorizontalAlignment', 'Source'], selfClosing: true, canClose: false }, // Updated
    'TextBlock': { attributes: ['FontSize', 'FontWeight', 'FontFamily', 'Width', 'HorizontalAlignment', 'Content'], selfClosing: true, canClose: true },
    'Barcode': { attributes: ['HorizontalAlignment', 'Width', 'Format', 'Content'], selfClosing: true, canClose: false }, // Updated
    'Dictionary': { attributes: [], selfClosing: true, canClose: true } // Removed trailing comma
};
console.log("Tag definitions updated:", tagDefinitions);

// Flags to prevent multiple registrations
window.monacoLanguageRegistered = window.monacoLanguageRegistered || false;
window.monacoThemeRegistered = window.monacoThemeRegistered || false;
window.monacoCompletionProviderRegistered = window.monacoCompletionProviderRegistered || false;

/**
 * Initializes the Monaco editor with RML support.
 * @param {string} editorId - The ID of the DOM element to host the editor.
 * @param {string} initialCode - The initial RML code to load into the editor.
 * @param {object} dotNetHelper - The .NET helper object for interop.
 */
window.initializeMonaco = (editorId, initialCode, dotNetHelper) => {
    console.log(`Initializing Monaco editor with ID: ${editorId}, initial code length: ${initialCode.length}`);

    // Configure RequireJS to load Monaco from a CDN
    require.config({ paths: { 'vs': 'https://unpkg.com/monaco-editor@latest/min/vs' } });
    require(['vs/editor/editor.main'], function () {
        if (window.monacoEditors[editorId]) {
            console.log(`Editor with ID ${editorId} already initialized.`);
            return;
        }

        // Register the custom 'rml' language only once
        if (!window.monacoLanguageRegistered) {
            console.log("Registering 'rml' language.");
            monaco.languages.register({ id: 'rml' });

            // Define the Monarch tokenizer for RML
            monaco.languages.setMonarchTokensProvider('rml', {
                tokenizer: {
                    root: [
                        [/(<\/?)([a-zA-Z][\w]*)(\s|>)/, ['delimiter', 'tag', 'delimiter']], // Opening/closing tags with delimiter
                        [/([a-zA-Z][\w]*)\s*=/, 'attribute.name'],                         // Attribute names
                        [/"/, 'attribute.value', '@attributeValue'],                       // Starting attribute value
                        [/<\/?/, 'delimiter'],                                             // Delimiters like '<' and '/'
                        [/\/?>/, 'delimiter'],                                             // Self-closing tag delimiter
                    ],
                    attributeValue: [
                        [/[^"]+/, 'attribute.value'],                                      // Attribute value content
                        [/"/, 'attribute.value', '@pop'],                                 // End of attribute value
                    ]
                },
            });
            window.monacoLanguageRegistered = true;
        } else {
            console.log("'rml' language already registered.");
        }

        // Define a custom theme only once
        if (!window.monacoThemeRegistered) {
            console.log("Defining 'customTheme'.");
            monaco.editor.defineTheme('customTheme', {
                base: 'vs-dark',
                inherit: true,
                rules: [
                    { token: 'tag', foreground: '#1263e6' },              // Blue for tag names
                    { token: 'attribute.name', foreground: '#1263e6' },   // Blue for attribute names
                    { token: 'attribute.value', foreground: '#C0C0C0' },  // Light gray for attribute values
                    { token: 'delimiter', foreground: '#808080' },        // Gray for delimiters
                ],
                colors: {
                    'editor.background': '#1E1E1E',
                },
            });
            window.monacoThemeRegistered = true;
        } else {
            console.log("'customTheme' already defined.");
        }

        // Create the Monaco editor instance
        const editor = monaco.editor.create(document.getElementById(editorId), {
            value: initialCode,
            language: 'rml',
            automaticLayout: true,
            theme: 'customTheme',
            autoClosingBrackets: 'never',
            autoClosingQuotes: 'never',
            overflowWidgets: true,
            padding: { top: 20 }
        });

        // Register a completion item provider only once
        if (!window.monacoCompletionProviderRegistered) {
            console.log("Registering completion item provider for 'rml'.");
            const completionProvider = monaco.languages.registerCompletionItemProvider('rml', {
                triggerCharacters: ['<', ' '],
                provideCompletionItems: (model, position) => {
                    const textUntilPosition = model.getValueInRange({
                        startLineNumber: position.lineNumber,
                        startColumn: 1,
                        endLineNumber: position.lineNumber,
                        endColumn: position.column
                    });

                    const suggestions = [];

                    // Extract all text before the current position to determine context
                    const textBefore = model.getValueInRange({
                        startLineNumber: 1,
                        startColumn: 1,
                        endLineNumber: position.lineNumber,
                        endColumn: position.column
                    });

                    // Function to determine if the cursor is inside a <DataContext> tag
                    const isInsideDataContext = (() => {
                        const openTags = (textBefore.match(/<DataContext\b[^>]*>/g) || []).length;
                        const closeTags = (textBefore.match(/<\/DataContext>/g) || []).length;
                        return openTags > closeTags;
                    })();

                    // Suggest tags when typing '<'
                    if (textUntilPosition.endsWith('<')) {
                        if (isInsideDataContext) {
                            // If inside <DataContext>, only suggest 'Dictionary' with closing tag
                            suggestions.push({
                                label: 'Dictionary (with closing tag)',
                                kind: monaco.languages.CompletionItemKind.Snippet,
                                insertText: `<Dictionary>$0</Dictionary>`,
                                insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                                detail: `Insert <Dictionary> tag with closing tag`
                            });
                        } else {
                            // If not inside <DataContext>, suggest all tags as usual
                            Object.entries(tagDefinitions).forEach(([tag, def]) => {
                                // If the tag can have a closing tag, provide the opening and closing snippet
                                if (def.canClose) {
                                    suggestions.push({
                                        label: `${tag} (with closing tag)`,
                                        kind: monaco.languages.CompletionItemKind.Snippet,
                                        insertText: `${tag}>$0</${tag}>`,
                                        insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                                        detail: `Insert <${tag}> tag with closing tag`
                                    });
                                }

                                // If the tag is self-closing, provide the self-closing option
                                if (def.selfClosing) {
                                    suggestions.push({
                                        label: `${tag} (self-closing)`,
                                        kind: monaco.languages.CompletionItemKind.Snippet,
                                        insertText: `<${tag} />$0`,
                                        insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                                        detail: `Insert self-closing <${tag} /> tag`
                                    });
                                }
                            });
                        }
                    }

                    // Suggest attributes based on the current tag
                    const tagMatch = textUntilPosition.match(/<(\w+)\s+[^>]*$/);
                    if (tagMatch) {
                        const tagName = tagMatch[1];
                        const tagDef = tagDefinitions[tagName];
                        const attributes = tagDef ? tagDef.attributes : [];

                        attributes.forEach(attr => {
                            suggestions.push({
                                label: attr,
                                kind: monaco.languages.CompletionItemKind.Property,
                                insertText: `${attr}="$1"`,
                                insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                                detail: `Add ${attr} attribute`
                            });
                        });
                    }

                    return { suggestions };
                }
            });

            // Store the disposable for potential future use
            window.monacoCompletionProviderDisposable = completionProvider;
            window.monacoCompletionProviderRegistered = true;
        } else {
            console.log("Completion item provider for 'rml' already registered.");
        }

        // Store the editor instance for later reference
        window.monacoEditors[editorId] = editor;

        // Optionally, notify .NET about the initial code load
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync('CodeChanged', initialCode)
                .catch(err => console.error("Error invoking CodeChanged:", err));
        }

        // Attach an event listener to handle content changes
        editor.onDidChangeModelContent(() => {
            const currentCode = editor.getValue();
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync('CodeChanged', currentCode)
                    .catch(err => console.error("Error invoking CodeChanged:", err));
            }
        });
    })
}; // Correctly closing initializeMonaco function

/**
    * Retrieves the current code from a specified Monaco editor.
    * @param {string} editorId - The ID of the Monaco editor.
    * @returns {string} - The current code in the editor.
    */
window.getMonacoCode = (editorId) => {
    const code = window.monacoEditors[editorId]?.getValue();
    return code;
};