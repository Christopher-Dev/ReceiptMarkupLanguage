// ====================== Add String to Line Method ======================

/**
 * Adds a string to a specified line number in the Monaco Editor.
 * @param {number} lineNumber - The 1-based line number where the text will be added.
 * @param {string} text - The string to add to the specified line.
 */
window.addStringToLine = function (lineNumber, text) {
    if (!window.editorInstance) {
        console.error("Monaco Editor instance does not exist.");
        return;
    }

    const model = window.editorInstance.getModel();
    if (!model) {
        console.error("Monaco Editor model does not exist.");
        return;
    }

    const totalLines = model.getLineCount();
    if (typeof lineNumber !== 'number' || lineNumber < 1 || lineNumber > totalLines) {
        console.error(`Invalid line number: ${lineNumber}. Must be between 1 and ${totalLines}.`);
        return;
    }

    if (typeof text !== 'string') {
        console.error(`Invalid text type: ${typeof text}. Must be a string.`);
        return;
    }

    // Get the content of the specified line
    const lineContent = model.getLineContent(lineNumber);
    const newContent = lineContent + text;

    // Create an edit operation to append the text at the end of the line
    const edit = {
        range: new monaco.Range(lineNumber, lineContent.length + 1, lineNumber, lineContent.length + 1),
        text: text
    };

    // Apply the edit to the model
    model.pushEditOperations([], [edit], () => null);

    console.log(`Added text to line ${lineNumber}: "${text}"`);

    // Optionally, you can move the cursor to the end of the inserted text
    window.editorInstance.setPosition({ lineNumber: lineNumber, column: lineContent.length + text.length + 1 });
    window.editorInstance.focus();
};

console.log("Method 'addStringToLine' has been added to the window object.");



function addCursorPositionListener() {
    if (window.editorInstance && !window.cursorPositionListenerAdded) {
        window.editorInstance.onDidChangeCursorPosition(function (e) {
            const position = window.editorInstance.getPosition();
            const lineNumber = position.lineNumber;

            console.log(`Cursor moved to line ${lineNumber}`);

            // **Invoke the C# method to update LinePosition**
            dotNetObject.invokeMethodAsync('UpdateLinePosition', lineNumber)
                .then(() => {
                    console.log(`LinePosition updated to ${lineNumber} in Blazor.`);
                })
                .catch(err => {
                    console.error("Error invoking 'UpdateLinePosition':", err);
                });
        });
        window.cursorPositionListenerAdded = true; // Flag to prevent duplicate listeners
        console.log("Cursor position change listener added.");
    } else if (!window.editorInstance) {
        console.warn("Monaco Editor instance does not exist. Listener not added.");
    } else {
        console.log("Cursor position listener already added.");
    }
}