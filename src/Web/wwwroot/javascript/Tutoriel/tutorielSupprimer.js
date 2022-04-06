(function () {
    'use strict';

    var idTuto = "";

    $('[data-supprimerTuto]').on('click', event => {
        event.preventDefault();
        event.stopPropagation();
        let boutton = event.currentTarget;

        idTuto = boutton.form[0].value
    });



    $('#btnConfirmDeleteTuto').click(function () {
        var href = window.location.pathname + "?handler=DeleteTuto";

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
                //window.location.replace(window.location + "&deleteStatus=true");
                //window.location.replace(window.location);
                window.location.replace('/Tutoriel?deleteStatus=true');


            },
            error: function () {
                //window.location.replace(window.location + "&deleteStatus=false");
                //window.location.replace(window.location);
                window.location.replace('/Tutoriel?deleteStatus=false');


            }
        });
    });

}());
