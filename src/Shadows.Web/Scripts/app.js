function ConfirmDelete() {
    return confirm("Opravdu smazat záznam?");
}

function ConfirmReset() {
    return confirm("Opravdu provést reset?");
}

function ShowBootstrapPopup(popupId) {
    var myModal = new bootstrap.Modal(document.getElementById(popupId));
    myModal.show();
}