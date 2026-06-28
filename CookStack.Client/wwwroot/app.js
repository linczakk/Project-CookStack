window.setDocumentTitle = (title) => {
    document.title = title;
};

window.keyboardShortcuts = {
    register: function (dotNetHelper) {

        document.addEventListener("keydown", function (e) {

            if (e.ctrlKey && e.key === "k") {
                e.preventDefault();
                dotNetHelper.invokeMethodAsync("OpenSearch", null);
            }

            if (e.key === "Escape") {
                dotNetHelper.invokeMethodAsync("EscapePressed");
            }

            if (e.key === "ArrowDown") {
                dotNetHelper.invokeMethodAsync("ArrowDownPressed");
            }

            if (e.key === "ArrowUp") {
                dotNetHelper.invokeMethodAsync("ArrowUpPressed");
            }

            if (e.key === "Enter") {
                dotNetHelper.invokeMethodAsync("EnterPressed");
            }
        });

        document.addEventListener("mousedown", () => {
            dotNetHelper.invokeMethodAsync("MouseUsed");
        });
    }
};