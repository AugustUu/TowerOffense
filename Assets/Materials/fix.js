function getCustomInnerText(element) {
    if (element.nodeType === Node.TEXT_NODE) {
        return element.nodeValue;
    }

    // Check if the element has the data-asciimath attribute
    if (element.nodeType === Node.ELEMENT_NODE && element.hasAttribute("data-asciimath")) {
        return element.getAttribute("data-asciimath");
    }

    // Recursively get inner text from children
    let text = "";
    for (let child of element.childNodes) {
        text += getCustomInnerText(child);
    }

    return text;
}

var lastout = null

document.addEventListener("keypress", (event) => {
    if (event.key == "=") {
        navigator.clipboard.read().then(clipboard => {
            clipboard[0].getType("text/html").then(data => {
                data.text().then(text => {
                    const parser = new DOMParser();
                    const doc = parser.parseFromString(text, "text/html");
                    let out = "";

                    Array.from(doc.body.children).forEach(item => {
                        out += getCustomInnerText(item)
                    });


                    const options = {
                        method: 'POST',
                        headers: {
                            Accept: '*/*',
                            'Accept-Language': 'en-US,en;q=0.9',
                            Connection: 'keep-alive',
                            'Content-Type': 'application/json',
                            Origin: 'null',
                            'Sec-Fetch-Dest': 'empty',
                            'Sec-Fetch-Mode': 'cors',
                            'Sec-Fetch-Site': 'cross-site',
                            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36',
                            'sec-ch-ua': '"Chromium";v="136", "Google Chrome";v="136", "Not.A/Brand";v="99"',
                            'sec-ch-ua-mobile': '?0',
                            'sec-ch-ua-platform': '"Windows"'
                        },
                        body: `{"msg":"${btoa(out)}"}`
                    };

                    fetch('https://cors-anywhere.herokuapp.com/http://f8fbe51347.zapto.org:48876/chat', options)
                        .then(response => response.text())
                        .then(response => {
                            lastout = response
                            navigator.clipboard.writeText(response)
                        })
                        .catch(err => navigator.clipboard.writeText(console.error(err)));

                });
            });

        });
    }

    if (event.key == "-") {
        navigator.clipboard.writeText(lastout.split("FINAL")[1])
    }
})
