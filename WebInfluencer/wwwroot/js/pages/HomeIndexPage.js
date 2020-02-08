var HomeIndexPage = (function () {

    function init() {
        handler();

        $('#main').css('min-height', '0');
    }


    function handler() {
        $('.owl-carousel').owlCarousel({
            margin: 10,
            loop: true,
            autoWidth: true,
            items: 4
        });
    }
    return {
        Init: init
    };

})();