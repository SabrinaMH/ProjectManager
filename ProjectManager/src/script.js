
function get(url) {
    return new Promise((resolve, reject) => {
        const req = new XMLHttpRequest();
        req.overrideMimeType('application/json');
        req.open('GET', url);
        req.onload = () => req.status === 200 ? resolve(JSON.parse(req.response)) : reject(Error(req.statusText));
        req.onerror = (e) => reject(Error(`Network Error: ${e}`));
        req.send();
    });
}

function populateProjectList() {
    get('project')
        .then((projects) => {
            var ul = $('<ul>').appendTo($('#projectList'));

            if (projects.length === 0) {
                return;
            }
            
            projects.forEach(function (project) {
                extendDomWithProject(ul, project);
            });

            var firstProjectItem = document.querySelector("ul > li");
            firstProjectItem.classList.add('selectedProject');
            selectedProjectId = projects[0].Id;
            populateTaskView(selectedProjectId);
        })
        .catch((err) => {
            console.log(err);
        });
}

function appendToProjectList(projectId) {
    get('project/' + projectId)
        .then((project) => {
            var ul = $('#projectList ul');
            extendDomWithProject(ul, project);
        })
        .catch((err) => {
            console.log(err);
        });
}

function extendDomWithProject(ul, project) {
    var li = $(document.createElement('li'));
    li[0].classList.add("clickable");
    li.text(project.Title);
    li.click(() => 
    {
        if (project.Id === selectedProjectId) return;

        var previouslySelectedProject = document.getElementsByClassName('selectedProject')[0];
        previouslySelectedProject.classList.remove('selectedProject');
        li[0].classList.add('selectedProject');
        selectedProjectId = project.Id;
        populateTaskView(project.Id);
    });
    ul.append(li);
}

function appendToTaskView(taskId) {
    get('/task/' + taskId)
        .then((task) => {
            extendDomWithTask(task);
        })
        .catch((err) => {
            console.log(err);
        });
}

function extendDomWithTask(task) {
    var taskTable = $('#taskTable tbody');
    var tr = $(document.createElement('tr'));
    tr[0].classList.add("clickable");
    tr[0].setAttribute('data-taskId', task.Id);
    tr.click(() => {
        if (task.Id === selectedTaskId) return;

        var previouslySelectedTask = document.getElementsByClassName('selectedTask')[0];
        if (previouslySelectedTask !== undefined) {
            previouslySelectedTask.classList.remove('selectedTask');
        }
        tr[0].classList.add('selectedTask');
        selectedTaskId = task.Id;
        populateNote(task.Id);
    });
    var td = document.createElement('td');
    td.append(document.createTextNode(task.Title));
    tr.append(td);
    td = document.createElement('td');
    var deadline = new Date(task.Deadline);
    td.append(document.createTextNode(deadline.toString("HH:mm d/M/yyyy")));
    tr.append(td);
    td = document.createElement('td');
    td.append(document.createTextNode(task.Done));
    tr.append(td);
    td = document.createElement('td');
    td.append(document.createTextNode(task.HasNote));
    tr.append(td);
    td = document.createElement('td');
    td.append(document.createTextNode(task.Priority));
    tr.append(td);
    taskTable.append(tr);
}

$('#addProjectForm').submit(function () {
    var title = $("#projectTitle").val().toString();
    var deadline = $("#addProjectForm #projectDeadline").val();
    var formData = { Title: title, Deadline: deadline }
    var formURL = $('#addProjectForm').attr("action");
    $.ajax(
        {
            url: formURL,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            success: function (projectId) {
                appendToProjectList(projectId);
                selectedProjectId = projectId;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown, jqXHR.reason);
            },
            complete: function() {
                $("#addProjectForm")[0].reset();
            }
        });
     return false;
});

$('#addTaskForm').submit(function () {
    var title = $("#addTaskForm #taskTitle").val().toString();
    var date = $("#deadlineDate").val();
    var time = $('#deadlineTime').val();
    var deadline = new Date(date + ' ' + time);
    var priority = $("#addTaskForm #priority").val();
    var formData = { ProjectId: selectedProjectId, Title: title, Deadline: deadline, Priority: priority }
    var formURL = $('#addTaskForm').attr("action");

    $.ajax(
        {
            url: formURL,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            success: function (taskId) {
                appendToTaskView(taskId);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown, jqXHR.reason);
            },
            complete: function () {
                $("#addTaskForm")[0].reset();
            }
        });
    return false;
});

$('#noteForm').submit(function () {
    var text = $("#noteTextBox").val();
    var formData = { TaskId: selectedTaskId, Text: text }
    var formURL = $('#noteForm').attr("action");
    $.ajax(
        {
            url: formURL,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            success: function (taskId) {
                alert("Saved note successfully");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown, jqXHR.reason);
            }
        });
    return false;
});

function fillPriorityDropDown() {
    $.ajax(
    {
        url: "/task/priority",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function(priorities) {
            var fragment = document.createDocumentFragment();
            priorities.forEach(function(task) {
                var opt = document.createElement('option');
                opt.innerHTML = task;
                opt.value = task;
                fragment.append(opt);
            });
            var priorityDropdown = document.getElementById('priority');
            priorityDropdown.append(fragment);
        },
        error: function(jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown, jqXHR.reason);
        }
    });
}

function populateTaskView(projectId) {
    $('#taskTable tbody').empty();
    get('project/' + projectId + '/task')
        .then((tasks) => {
            if (tasks.length === 0) {
                $('#noteTextBox').empty();
                return;
            }
            
            for (var i = 0; i < tasks.length; i++) {
                extendDomWithTask(tasks[i]);
            }
            
            var firstTaskItem = document.querySelector("tbody > tr");
            firstTaskItem.classList.add('selectedTask');
            populateNote(tasks[0].Id);
        })
        .catch((err) => {
            console.log(err);
        });
}

function showContextMenuWhenRightClickingOnTask() {
    $('#taskTable').on('contextmenu', function (e) {
        e.preventDefault();
        e.stopPropagation();
        if ($(e.target).is('td')) {
            selectedTaskId = e.target.parentNode.getAttribute('data-taskId');
            $('#taskContextMenu').css({
                top: e.pageY + 'px',
                left: e.pageX + 'px'
            }).show();
        }
        return false;
    });

    $(document).on('click',
        function (e) {
            if (!$(e.target).is('#taskContextMenu li'))
                $('#taskContextMenu').hide();
        });
}

function populateNote(taskId) {
    $('#noteTextBox').empty();
    get('task/' + taskId + '/note')
        .then((note) => {
            $('#noteTextBox').text(note.Text);
        })
        .catch((err) => {
            console.log(err);
        });
}

var selectedProjectId;
var selectedTaskId;

fillPriorityDropDown();
populateProjectList();
//showContextMenuWhenRightClickingOnTask();
