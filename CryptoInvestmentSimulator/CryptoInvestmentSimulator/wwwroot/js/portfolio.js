
// Functions to show/hide user details editing popup.
function hideEditDetailsModal()
{
    $('#exampleModalCenter').modal('hide');
}

function showEditDetailsModal()
{
    $('#exampleModalCenter').modal('show');
}

$(document).ready(function ()
{
    hideEditDetailsModal();

    document.getElementById("edit").addEventListener("click", showEditDetailsModal);
    document.getElementById("cancel").addEventListener("click", hideEditDetailsModal);
    document.getElementById("save").addEventListener("click", hideEditDetailsModal);
});
