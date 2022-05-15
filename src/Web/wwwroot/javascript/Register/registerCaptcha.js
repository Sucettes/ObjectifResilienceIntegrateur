function onSubmit(token) {
    document.getElementById("demo-test").submit();
}

$('[data-registerRequest]').on('click', event => {
    document.getElementById('msgErreurPassword').textContent = "";
    document.getElementById('msgErreurPasswordLength').textContent = "";
    document.getElementById('msgErreurConfPassword').textContent = "";
    document.getElementById('msgErreurCourriel').textContent = "";
    if (document.getElementById('password').value != "" && document.getElementById('confPassword').value != "" && document.getElementById('courriel').value != "") {
        $.ajax({
            type: 'POST',
            url: window.location.pathname,
            async: false,
            data: new FormData(document.getElementById('form-register')),
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.msgSuccess != null) {
                    window.scriptToastNotification.AjouterNotification("Demande d'inscription effectué ! Vous allez être redirigé vers la page d'accueil dans 10 secondes.", true);
                    setTimeout(function () {
                        window.location.href = '/Index';
                    }, 10000);

                }
                else {
                    if (data.erreurPassword != null) {
                        document.getElementById('msgErreurPassword').textContent = data.erreurPassword;
                        document.getElementById('msgErreurConfPassword').textContent = data.erreurPassword;
                    }
                    if (data.erreurCourriel != null) {
                        document.getElementById('msgErreurCourriel').textContent = data.erreurCourriel;
                    }
                    window.scriptToastNotification.AjouterNotification("Une erreure est survenu lors de la demande d'inscription !", false);

                }
            },
            error: function () {
                window.scriptToastNotification.AjouterNotification("Une erreure est survenu lors de la demande d'inscription !", false);
            }
        });
    }
    else {
        if (document.getElementById('password').value == "") {
            document.getElementById('msgErreurPassword').textContent = "Le champ ne peut pas être vide.";
        }
        if (document.getElementById('confPassword').value == "") {
            document.getElementById('msgErreurConfPassword').textContent = "Le champ ne peut pas être vide.";
        }
        if (document.getElementById('courriel').value == "") {
            document.getElementById('msgErreurCourriel').textContent = "Le champ ne peut pas être vide.";
        }
    }
});