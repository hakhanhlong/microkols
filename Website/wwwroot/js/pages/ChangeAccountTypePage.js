var ChangeAccountTypePage = (function () {

    function init() {
        handler();
    }

    function handler() {
        /*
        var datas = $.parseJSON($('#modelHotMomData').html());
        
        var vm = new Vue({
            el: '#dataHotMom',
            data: {
                childrens: []
            },
            created() {
                this.childrens = datas;
            },
            methods: {
                createItem: function () {
                    this.childrens.push({
                        Gender: 1,
                        Age: 0,
                        AgeType: 2
                    });
                },
                removeItem: function (idx) {
                    this.data.splice(idx, 1);
                }

            }
        });
        */
        handlerType();
        $("input[name=Type]").change(function () {

            handlerType();
        });


        $('.checkbox-ignorecampaigntype').change(function () {
            var url = $(this).data('url');
            $.post(url, function () { });

        });

    }

    function handlerType() {
        var type = $("input[name=Type]:checked").val();
        if (type === 'HotMom') {
            $('#dataHotMom').removeClass('d-none');
        } else {
            $('#dataHotMom').addClass('d-none');
        }
    }
    return {
        Init: init
    };

})();
