const offer = {
    init: function () {
        $("#addOfferButton").on("click", function () {
            $("#offerForm").show();
            $("#addOfferButton").hide();
        });

        $("#addNewOfferButton").on("click", function () {
            $("#offerForm").hide();
            $("#newOfferForm").show();
        });
    },
};