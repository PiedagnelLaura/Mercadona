const app = {
    init: function () {
        filter.init();
        offer.init();
    }
};

document.addEventListener('DOMContentLoaded', app.init);