

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


    tinymce.init({
        selector: '.form-tinymce',  // change this value according to your HTML
        height: 350,
        plugins: 'print preview  searchreplace autolink directionality  visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount   imagetools   contextmenu colorpicker textpattern help',
        toolbar1: 'formatselect |  fontselect fontsizeselect | bold italic strikethrough forecolor backcolor  | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat',
        image_advtab: true,
        convert_urls: false,
        fontsize_formats: "8pt 10pt 12pt 14pt 18pt 24pt 36pt",
        content_css: '//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css',
        images_upload_handler: function (blobInfo, success, failure) {
            var xhr, formData;

            xhr = new XMLHttpRequest();
            xhr.withCredentials = false;
            xhr.open('POST', '/api/editorimageupload');

            xhr.onload = function () {


                if (xhr.status != 200) {
                    failure('HTTP Error: ' + xhr.status);
                    return;
                }

                var json = JSON.parse(xhr.responseText);

                if (!json) {
                    failure('Invalid JSON: ' + xhr.message);
                    return;
                }

                if (json.status == 1) {
                    success(json.imageurl);
                } else {
                    failure('error: ' + json.message);

                }

            };

            formData = new FormData();
            formData.append('file', blobInfo.blob(), blobInfo.filename());

            xhr.send(formData);
        }
    });


});

