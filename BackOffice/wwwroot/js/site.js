

$(document).ready(function () {
    $('#sl-entity-type').change(function () {

        if ($(this).val() === 'Account') {
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
