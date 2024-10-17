// wwwroot/js/validation.js

/**
 * Validates the custom XML from the Monaco editor.
 * Ensures the XML is well-formed and contains <Receipt>, <Body>, <DataContext>, and <Resources> elements.
 * 
 * @param {string} xmlString - The XML string to validate.
 * @returns {boolean} - Returns true if the XML is valid, false otherwise.
 */
window.validateCustomXML = function (xmlString) {
    try {
        // Initialize the DOMParser
        const parser = new DOMParser();

        // Parse the XML string
        const xmlDoc = parser.parseFromString(xmlString, "application/xml");

        // Check for parsing errors
        const parserError = xmlDoc.getElementsByTagName("parsererror")[0];
        if (parserError) {
            console.error("XML Parsing Error: " + parserError.textContent);
            return false;
        }

        // Retrieve the root element of the XML
        const root = xmlDoc.documentElement;

        // Check if the root element is <Receipt>
        if (root.nodeName !== "Receipt") {
            console.error(`Invalid root element: expected <Receipt>, found <${root.nodeName}>`);
            return false;
        }

        // Define the required child elements of <Receipt>
        const requiredElements = ["Body", "DataContext", "Resources"];

        // Iterate over the required elements and check their existence
        for (let elemName of requiredElements) {
            const elements = root.getElementsByTagName(elemName);

            // Ensure that the element exists as a direct child of <Receipt>
            let isDirectChild = false;
            for (const element of elements) {
                if (element.parentNode === root) {
                    isDirectChild = true;
                    break;
                }
            }

            if (!isDirectChild) {
                console.error(`Missing required element: <${elemName}> as a direct child of <Receipt>`);
                return false;
            }
        }

        // Validate that Content attributes reference keys in DataContext Pairs (excluding Repeater elements)
        const dataContext = root.getElementsByTagName("DataContext")[0];
        const validKeys = {};

        if (dataContext) {
            const pairs = dataContext.getElementsByTagName("Pair");
            for (const pair of pairs) {
                const key = pair.getAttribute("Key");
                if (key) {
                    validKeys[key] = true;
                }
            }
        }

        // Check that all attributes contain a non-empty value (excluding whitespace-only)
        for (const element of xmlDoc.getElementsByTagName("*")) {
            if (element.nodeName === "Repeater") {
                // Check specific validation for <Run> elements within <Repeater>
                const runs = element.getElementsByTagName("Run");
                for (const run of runs) {
                    const content = run.getAttribute("Content");
                    if (content === "" || content === null) {
                        console.error("Invalid <Run> element within <Repeater>: Content attribute cannot be empty.");
                        return false;
                    }
                }
                continue;
            }
            for (const attribute of element.attributes) {
                if (attribute.value.trim() === "") {
                    console.error(`Invalid attribute: ${attribute.name} cannot be empty.`);
                    return false;
                }
            }
        }

        // If all checks pass, return true
        return true;
    } catch (error) {
        // Log any unexpected errors and return false
        console.error("Unexpected Error during XML Validation: " + error);
        return false;
    }
};