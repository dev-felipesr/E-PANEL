"use strict";
function getCsrfToken() {
    const tokenMeta = document.querySelector('meta[name="csrf-token"]');
    return (tokenMeta === null || tokenMeta === void 0 ? void 0 : tokenMeta.content) || null;
}
function hasOpenOverlayOwner() {
    return Boolean(document.querySelector("dialog[open], .modal.show, .offcanvas.show, .offcanvas.showing"));
}
function cleanupUnexpectedBackdrop() {
    if (hasOpenOverlayOwner()) {
        return;
    }
    document.querySelectorAll(".modal-backdrop, .offcanvas-backdrop, [class*=\"backdrop\"]")
        .forEach(backdrop => {
        if (backdrop instanceof HTMLElement && backdrop !== document.body && backdrop !== document.documentElement) {
            backdrop.remove();
        }
    });
    if (document.body.classList.contains("modal-open")) {
        document.body.classList.remove("modal-open");
        document.body.style.removeProperty("overflow");
        document.body.style.removeProperty("padding-right");
    }
}
async function sendVote(container, value) {
    const id = Number(container.dataset.id);
    const url = container.dataset.url;
    const csrfToken = getCsrfToken();
    if (!id || !url || !csrfToken) {
        return;
    }
    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "X-CSRF-TOKEN": csrfToken
        },
        body: JSON.stringify({ id, value })
    });
    if (!response.ok) {
        return;
    }
    const data = await response.json();
    const scoreEl = container.querySelector(".score-value");
    if (scoreEl) {
        scoreEl.textContent = String(data.score);
    }
    const upButton = container.querySelector('.vote-btn[data-value="1"]');
    const downButton = container.querySelector('.vote-btn[data-value="-1"]');
    if (upButton) {
        upButton.classList.toggle("btn-success", data.userVote === 1);
        upButton.classList.toggle("btn-outline-success", data.userVote !== 1);
    }
    if (downButton) {
        downButton.classList.toggle("btn-danger", data.userVote === -1);
        downButton.classList.toggle("btn-outline-danger", data.userVote !== -1);
    }
}
document.querySelectorAll(".vote-controls").forEach(container => {
    container.querySelectorAll(".vote-btn").forEach(button => {
        button.addEventListener("click", async (event) => {
            event.preventDefault();
            event.stopPropagation();
            const value = Number(button.dataset.value);
            if (value !== 1 && value !== -1) {
                return;
            }
            try {
                await sendVote(container, value);
            }
            finally {
                button.blur();
                cleanupUnexpectedBackdrop();
            }
        });
    });
});
document.addEventListener("click", () => {
    window.setTimeout(cleanupUnexpectedBackdrop, 0);
}, true);
document.addEventListener("hidden.bs.modal", cleanupUnexpectedBackdrop);
document.addEventListener("hidden.bs.offcanvas", cleanupUnexpectedBackdrop);
const observer = new MutationObserver(() => cleanupUnexpectedBackdrop());
observer.observe(document.body, { childList: true, subtree: true, attributes: true, attributeFilter: ["class"] });
cleanupUnexpectedBackdrop();
