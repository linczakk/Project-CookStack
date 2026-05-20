window.setDocumentTitle = (title) => {
    document.title = title;
};

window.keyboardShortcuts = {
    register: function (dotNetHelper) {

        document.addEventListener("keydown", function (e) {

            if (e.ctrlKey && e.key === "k") {
                e.preventDefault();
                dotNetHelper.invokeMethodAsync("OpenSearch");
            }

            if (e.key === "Escape") {
                dotNetHelper.invokeMethodAsync("EscapePressed");
            }
        });
    }
};