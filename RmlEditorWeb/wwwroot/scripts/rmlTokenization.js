// Ensure the Monaco editor object is initialized
window.monacoLanguageRegistered = window.monacoLanguageRegistered || false;
window.monacoThemeRegistered = window.monacoThemeRegistered || false;

/**
 * Registers the custom RML language and defines tokenization.
 * Also defines a custom theme for the Monaco editor.
 * @param {boolean} debug - Flag to enable or disable console logging.
 */
window.registerRmlLanguage = (debug = false) => {

    // Helper function for conditional logging
    const log = (message, ...optionalParams) => {
        if (debug) {
            console.log(message, ...optionalParams);
        }
    };

    if (!window.monacoLanguageRegistered) {
        log("Registering 'rml' language.");
        monaco.languages.register({ id: 'rml' });

        // Define the Monarch tokenizer for RML
        monaco.languages.setMonarchTokensProvider('rml', {
            tokenizer: {
                root: [
                    [/(<\/?)([a-zA-Z][\w]*)(\s|>)/, ['delimiter', 'tag', 'delimiter']],
                    [/([a-zA-Z][\w]*)\s*=/, 'attribute.name'],
                    [/"/, 'attribute.value', '@attributeValue'],
                    [/<\/?/, 'delimiter'],
                    [/\/?>/, 'delimiter'],
                ],
                attributeValue: [
                    [/[^"]+/, 'attribute.value'],
                    [/"/, 'attribute.value', '@pop'],
                ]
            },
        });
        window.monacoLanguageRegistered = true;
    } else {
        log("'rml' language already registered.");
    }

    // Define a custom theme only once
    if (!window.monacoThemeRegistered) {
        log("Defining 'customTheme'.");
        monaco.editor.defineTheme('customTheme', {
            base: 'vs-dark',
            inherit: true,
            rules: [
                { token: 'tag', foreground: '#1263e6' },
                { token: 'attribute.name', foreground: '#1263e6' },
                { token: 'attribute.value', foreground: '#C0C0C0' },
                { token: 'delimiter', foreground: '#808080' },
            ],
            colors: {
                'editor.background': '#1E1E1E',
            },
        });
        window.monacoThemeRegistered = true;
    } else {
        log("'customTheme' already defined.");
    }
};
