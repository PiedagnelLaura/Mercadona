const filter = {
    init: function () {
        const buttonFilterElmt = document.querySelector('#filter');
        if (buttonFilterElmt) {
            buttonFilterElmt.addEventListener('click', filter.handleClickFilter);
        }

        const categorySelect = document.getElementById("categorySelect");
        if (categorySelect) {
            categorySelect.addEventListener("change", filter.handleSelectCategories);
        }
    },

    /**
     * when you click on the "filter" button, a div appears
     */
    handleClickFilter: function (evt) {
        evt.preventDefault();
        document.querySelector('.filter-case').classList.toggle('d-none');
    },

    /**
     * products are filtered by select
     */
    handleSelectCategories: function (evt) {
        let selectedValue = categorySelect.value;

        let productList = document.querySelectorAll('.product');

        for (const product of productList) {
            product.classList.add('d-none');

            if (selectedValue == "cat-all") {
                product.classList.remove('d-none');
            }
            else if (product.classList.contains(selectedValue)) {
                product.classList.remove('d-none');
            }
        }
    },
};