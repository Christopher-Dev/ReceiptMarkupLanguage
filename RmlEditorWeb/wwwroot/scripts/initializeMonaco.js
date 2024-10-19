// Ensure the Monaco editors object is initialized
window.monacoEditors = window.monacoEditors || {};
window.monacoCompletionProviderRegistered = window.monacoCompletionProviderRegistered || false;

/**
 * Initializes the Monaco editor with RML support.
 * @param {string} editorId - The ID of the DOM element to host the editor.
 * @param {string} initialCode - The initial RML code to load into the editor.
 * @param {object} dotNetHelper - The .NET helper object for interop.
 * @param {boolean} debug - Flag to enable or disable console logging.
 */
window.initializeMonaco = (editorId, initialCode, dotNetHelper, debug = false) => {

    // Helper function to handle conditional logging
    const log = (message, ...optionalParams) => {
        if (debug) {
            console.log(message, ...optionalParams);
        }
    };

    log(`Initializing Monaco editor with ID: ${editorId}, initial code length: ${initialCode.length}`);

    // Configure RequireJS to load Monaco from a CDN
    require.config({ paths: { 'vs': 'https://unpkg.com/monaco-editor@latest/min/vs' } });
    require(['vs/editor/editor.main'], function () {
        if (window.monacoEditors[editorId]) {
            log(`Editor with ID ${editorId} already initialized.`);
            return;
        }

        // Ensure the custom 'rml' language and theme are registered
        window.registerRmlLanguage(debug);

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
            log("Registering completion item provider for 'rml'.");

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

                    const textBefore = model.getValueInRange({
                        startLineNumber: 1,
                        startColumn: 1,
                        endLineNumber: position.lineNumber,
                        endColumn: position.column
                    });

                    const isInsideDataContext = (() => {
                        const openTags = (textBefore.match(/<DataContext\b[^>]*>/g) || []).length;
                        const closeTags = (textBefore.match(/<\/DataContext>/g) || []).length;
                        return openTags > closeTags;
                    })();

                    if (textUntilPosition.endsWith('<')) {
                        if (isInsideDataContext) {
                            suggestions.push({
                                label: 'Dictionary (with closing tag)',
                                kind: monaco.languages.CompletionItemKind.Snippet,
                                insertText: `<Dictionary>$0</Dictionary>`,
                                insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                                detail: `Insert <Dictionary> tag with closing tag`
                            });
                        } else {
                            Object.entries(tagDefinitions).forEach(([tag, def]) => {
                                if (def.canClose) {
                                    suggestions.push({
                                        label: `${tag} (with closing tag)`,
                                        kind: monaco.languages.CompletionItemKind.Snippet,
                                        insertText: `<${tag}>$0</${tag}>`,
                                        insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                                        detail: `Insert <${tag}> tag with closing tag`
                                    });
                                }

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

            window.monacoCompletionProviderDisposable = completionProvider;
            window.monacoCompletionProviderRegistered = true;
        } else {
            log("Completion item provider for 'rml' already registered.");
        }

        window.monacoEditors[editorId] = editor;

        // Optionally, notify .NET about the initial code load
        //if (dotNetHelper) {
        //    dotNetHelper.invokeMethodAsync('CodeChanged', initialCode)
        //        .catch(err => console.error("Error invoking CodeChanged:", err));
        //}

        //// Attach an event listener to handle content changes
        //editor.onDidChangeModelContent(() => {
        //    const currentCode = editor.getValue();
        //    if (dotNetHelper) {
        //        dotNetHelper.invokeMethodAsync('CodeChanged', currentCode)
        //            .catch(err => console.error("Error invoking CodeChanged:", err));
        //    }
        //});

    });
};

window.getMonacoCode = (editorId) => {
    const code = window.monacoEditors[editorId]?.getValue();
    return code;
};
