(function () {
    'use strict';

    var idTuto = "";
    var btnName = "";
    $('[data-UnpublishTuto]').on('click', event => {
        event.preventDefault();
        event.stopPropagation();
        let boutton = event.currentTarget;
        btnName = boutton.form[1].name;
        idTuto = boutton.form[0].value;
    });



    $('#btnConfirmUnpublishTuto').click(function () {
        var href = window.location.pathname + "?handler=UnpublishTuto";
        var tutoVal = { 'tutorielIdVal': idTuto };

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
                if (btnName === 'public') {
                    window.location.replace('/Tutoriel?unPublishStatus=false');
                } else if (btnName === 'nonPublic') {
                    window.location.replace('/Tutoriel?unPublishStatus=true');
                }
            },
            error: function () {
            }
        });
    });

}());
