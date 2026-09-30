window.scrollToBottom = (selector) => {
    const el = document.querySelector(selector);
    if (el) {
        el.scrollTop = el.scrollHeight;
    }
};
