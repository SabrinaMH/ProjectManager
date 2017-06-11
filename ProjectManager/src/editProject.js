
$('#editProjectForm').submit(function () {
    var title = $("#title").val().toString();
    var date = $("#deadlineDate").val();
    var time = $('#deadlineTime').val();
    var deadline = new Date(date + ' ' + time);
    var formData = { ProjectId: selectedProjectId, Title: title, Deadline: deadline }

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
    $('#deadlineDate').val(extractDate(deadline));
    $('#deadlineTime').val(extractTime(deadline));
}

function extractDate(dateTime) {
    var date = dateTime.split(" ")[1];
    var splittedDate = date.split("/");
    var day = splittedDate[0];
    var month = splittedDate[1];
    var year = splittedDate[2];
    if (month.length === 1) month = "0" + month;
    if (day.length === 1) day = "0" + day;
    return year + "-" + month + "-" + day;
}

function extractTime(dateTime) {
    var time = dateTime.split(" ")[0];
    return time;
}

var selectedProjectId = window.opener.selectedProjectId;
loadProject(selectedProjectId);
 