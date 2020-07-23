

$(document).ready(function () {
    $('#sl-entity-type').change(function () {
        if ($(this).val() === '1') {
            $('#div-select-type').show();
        }
        else {
            $('#div-select-type').hide();
        }
    });
});

function ShowHistory(transactionid, walletid) {
    var url = '/transaction/history?transactionid=' + transactionid + "&walletid=" + walletid;
    $('#iframehistory').attr('src', url);
    $('#m_modal_5').modal({ show: true });

    
}

function UpdateTransactionStatus(status, id) {



    var url = '/transaction/TransactionUpdateStatus?id=' + id + "&status=" + status;
    $('#iframenotestatus').attr('src', url);
    $('#m_modal_4').modal({ show: true });
    $('#m_modal_4').on('hidden.bs.modal', function (e) {
        document.location = document.location.href;
    });




    //$.ajax({
    //    type: "POST",        
    //    url: "/Transaction/ChangeStatus",
    //    data: { status: status, id:id },
    //    success: function (data) {

            

    //        var json_data = JSON.parse(data);

    //        if (json_data.Code === 1) {
                
    //            $('#transaction_' + id).remove();
    //            $('#transaction_note_' + id).remove();
                
    //        }
    //    },
    //    dataType: "json",
    //    traditional: true
    //});

}

$(function () {
    $(".delete-item").click(function (e) {
        confirm("Are you sure want delete this role ?");
        e.preventDefault();
        const anchor = $(this);
        const url = $(anchor).attr("href");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processData: false,
            type: "DELETE",
            url: url,
            success: function () {
                $(anchor).parent("td").parent("tr").fadeOut("slow",
                    function () {
                        $(this).remove();
                    });
            },
            error: function (xmlHttpRequest, textStatus, errorThrown) {
                let message = `${textStatus} ${xmlHttpRequest.status} ${errorThrown}`;
                if (xmlHttpRequest.responseText !== null) {
                    const response = JSON.parse(xmlHttpRequest.responseText);
                    for (let error of response["Error"]) {
                        message += `\n${error}`;
                    }
                }

                alert(message);
            }
        });
    });

    $("#tree").bonsai({
        expandAll: false,
        checkboxes: true,
        createInputs: "checkbox"
    });

    $("form").submit(function () {
        let controllerIndex = 0, actionIndex = 0;
        $('.controller > input[type="checkbox"]:checked, .controller > input[type="checkbox"]:indeterminate').each(function () {
            const controller = $(this);
            if ($(controller).prop("indeterminate")) {
                $(controller).prop("checked", true);
            }
            const controllerName = "SelectedControllers[" + controllerIndex + "]";
            $(controller).prop("name", controllerName + ".Name");

            const area = $(controller).next().next();
            $(area).prop("name", controllerName + ".AreaName");

            $('ul > li > input[type="checkbox"]:checked', $(controller).parent()).each(function () {
                const action = $(this);
                const actionName = controllerName + ".Actions[" + actionIndex + "].Name";
                $(action).prop("name", actionName);
                actionIndex++;
            });
            actionIndex = 0;
            controllerIndex++;
        });

        return true;
    });
});

