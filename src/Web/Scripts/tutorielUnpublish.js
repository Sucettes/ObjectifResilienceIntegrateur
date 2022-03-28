(function () {
    'use strict';

    var idTuto = "";

    $('[data-UnpublishTuto]').on('click', event => {
        event.preventDefault();
        event.stopPropagation();
        let boutton = event.currentTarget;

        idTuto = boutton.form[0].value
    });



    $('#btnConfirmUnpublishTuto').click(function () {
        var href = window.location.pathname + "?handler=UnpublishTuto";
        var tutoVal = { 'tutorielIdVal': idTuto }

        $.ajax({
            type: 'POST',
            url: href,
            data: JSON.stringify(tutoVal),
            contentType: "application/json",
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                //window.location.replace(window.location + "?unPublishStatus=true");
                //window.location.replace(window.location);
                window.location.replace('/Tutoriel?unPublishStatus=true')

            },
            error: function () {
                //window.location.replace(window.location + "?unPublishStatus=false");
                //window.location.replace(window.location);
                window.location.replace('/Tutoriel?unPublishStatus=false')

            }
        });
    });

}());