function UpdateTransactionStatus(status, id) {

    $.ajax({
        type: "POST",        
        url: "/Transaction/ChangeStatus",
        data: { status: status, id:id },
        success: function (data) {

            console.log(data);

            var json_data = JSON.parse(data);

            if (json_data.Code === 1) {
                console.log("Success");
                $('#transaction_' + id).remove();
                $('#transaction_note_' + id).remove();
                
            }
        },
        dataType: "json",
        traditional: true
    });

}
