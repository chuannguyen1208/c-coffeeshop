export const toast = (text, type = '' | 'text-success' | 'text-danger', closeAfterMs = 2000) => {
    const toast = document.querySelector(".toast");

    if (toast) {

        const body = toast.querySelector(".toast-body");
        body.innerHTML = text;
        body.classList.add(type);

        toast.classList.add('show');
        setTimeout(function () {
            toast.classList.remove('show');
        }, closeAfterMs);
    }
}

export const confirmDialog = (message) => {
    return confirm(message);
}

window.toast = toast;
window.confirmDialog = confirmDialog;