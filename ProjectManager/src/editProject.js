
$('#editProjectForm').submit(function () {
    var title = $("#title").val().toString();
    var formData = { ProjectId: selectedProjectId, Title: title}

    $.ajax(
        {
            url: "project/" + selectedProjectId,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            success: function () {
                window.opener.refreshTask(selectedProjectId);
                close();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown, jqXHR.reason);
            },
            complete: function () {
                $("#editProjectForm")[0].reset();
            }
        });
    return false;
});


function loadProject(id) {
    var project = window.opener.document.querySelector("[data-id='" + id + "']");
    var title = project.textContent;
    $('#title').val(title);
}

var selectedProjectId = window.opener.selectedProjectId;
loadProject(selectedProjectId);
 