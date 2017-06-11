
$('#editTaskForm').submit(function () {
    var title = $("#taskTitle").val().toString();
    var date = $("#deadlineDate").val();
    var time = $('#deadlineTime').val();
    var deadline = new Date(date + ' ' + time);
    var priority = $("#priority").val();
    var formData = { TaskId: selectedTaskId, Title: title, Deadline: deadline, Priority: priority }

    $.ajax(
        {
            url: "task/" + selectedTaskId,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            success: function () {
                window.opener.refreshTask(selectedTaskId);
                close();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown, jqXHR.reason);
            },
            complete: function () {
                $("#editTaskForm")[0].reset();
            }
        });
    return false;
});


function loadTask(id) {
    var taskTableRow = window.opener.document.querySelector("[data-id='" + id + "']");
    var title = taskTableRow.children[0].textContent;
    var deadline = taskTableRow.children[1].textContent;
    var done = taskTableRow.children[2].textContent;
    var hasNote = taskTableRow.children[3].textContent;
    var priority = taskTableRow.children[4].textContent;

    $('#taskTitle').val(title);
    $('#deadlineDate').val(extractDate(deadline));
    $('#deadlineTime').val(extractTime(deadline));
    $('#done').val(done);
    $('#hasNote').val(hasNote);
    document.getElementById('priority').value = priority;
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

function fillPriorityDropDown(callback) {
    return $.ajax(
        {
            url: "/task/priority",
            type: "GET",
            contentType: "application/json; charset=utf-8",
            success: function (priorities) {
                var fragment = document.createDocumentFragment();
                priorities.forEach(function (task) {
                    var opt = document.createElement('option');
                    opt.innerHTML = task;
                    opt.value = task;
                    fragment.append(opt);
                });
                var priorityDropdown = document.getElementById('priority');
                priorityDropdown.append(fragment);
                callback();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown, jqXHR.reason);
            }
        });
}

var selectedTaskId = window.opener.selectedTaskId;
fillPriorityDropDown(() => loadTask(selectedTaskId));
 