(function () {
    'use strict';

    var idTuto = "";
    var idRangee = "";

    $('[data-supprimer]').on('click', event => {
        event.preventDefault();
        event.stopPropagation();
        let boutton = event.currentTarget;

        idRangee = boutton.form[0].value
        idTuto = boutton.form[1].value
    });



    $('#btnConfirmDelete').click(function () {
        var href = window.location.pathname + "?id=" + idTuto + "&handler=DeleteRange";
        var rangee = { 'idRangeeVal': idRangee }

        $.ajax({
            type: 'POST',
            url: href,
            data: JSON.stringify(rangee),
            contentType: "application/json",
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                window.location.replace(window.location + "&deleteRowResult=true");

            },
            error: function () {
                window.location.replace(window.location + "&deleteRowResult=false");
            }
        });
    });

}());