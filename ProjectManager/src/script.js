
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
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown, jqXHR.reason);
            },
            complete: function () {
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
    var text = document.getElementById("noteTextBox").value;
    var formData = { TaskId: selectedTaskId, Text: text }
    var formURL = $('#noteForm').attr("action");
    $.ajax(
        {
            url: formURL,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            success: function () {
                refreshTask(selectedTaskId);
                alert("Saved note successfully");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown, jqXHR.reason);
            }
        });
    return false;
});

function populateProjectList() {
    get('project')
        .then((projects) => {
            var ul = $('<ul>').appendTo($('#projectList'));

            if (projects.length === 0) {
                return;
            }

            projects.sort((p1, p2) => { return p1.Title > p2.Title });
            projects.forEach(function (project) {
                extendDomWithProject(ul, project);
            });

            var firstProjectItem = document.querySelector("ul > li");
            firstProjectItem.classList.add('selectedProject');
            selectProject(projects[0].Id);
        })
        .catch((err) => {
            console.log(err);
        });
}

function populateTaskView(projectId) {
    $('#taskTable tbody').empty();
    get('project/' + projectId + '/task')
        .then((tasks) => {
            if (tasks.length === 0) {
                selectedTaskId = undefined;
                $('#noteTextBox').val('');
                return;
            }

            for (var i = 0; i < tasks.length; i++) {
                extendDomWithTask(tasks[i]);
            }

            selectTask(tasks[0].Id);
        })
        .catch((err) => {
            console.log(err);
        });
}

function populateNote(taskId) {
    document.getElementById('noteTextBox').value = "";
    get('task/' + taskId + '/note')
        .then((note) => {
            if (note === null) return;
            document.getElementById('noteTextBox').value = note.Text;
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
            selectProject(projectId);
        })
        .catch((err) => {
            console.log(err);
        });
}

function appendToTaskView(id) {
    get('/task/' + id)
        .then((task) => {
            extendDomWithTask(task);
            selectTask(id);
        })
        .catch((err) => {
            console.log(err);
        });
}

function refreshTask(id) {
    get('/task/' + id)
        .then((task) => {
            var taskTableRow = document.querySelector("[data-id='" + id + "']");
            taskTableRow.children[0].textContent = task.Title;
            taskTableRow.children[1].textContent = formatDeadline(task.Deadline);
            taskTableRow.children[2].textContent = task.Done;
            taskTableRow.children[3].textContent = task.HasNote;
            taskTableRow.children[4].textContent = task.Priority;
        })
        .catch((err) => {
            console.log(err);
        });
}

function formatDeadline(deadline) {
    var formattedDeadline = "";
    if (deadline !== null) {
        formattedDeadline = new Date(deadline).toString("HH:mm d/M/yyyy");
    }
    return formattedDeadline;
}

function extendDomWithProject(ul, project) {
    var li = $(document.createElement('li'));
    li.attr('data-id', project.Id);
    li[0].classList.add("clickable");
    li.text(project.Title);
    li.click(() => 
    {
        selectProject(project.Id);
    });
    ul.append(li);
}

function extendDomWithTask(task) {
    var taskTable = $('#taskTable tbody');
    var tr = $(document.createElement('tr'));
    tr.attr('data-id', task.Id);
    tr[0].classList.add("clickable");
    tr[0].setAttribute('data-taskId', task.Id);
    tr.click(() => {
        selectTask(task.Id);
    });
    var td = document.createElement('td');
    td.append(document.createTextNode(task.Title));
    tr.append(td);
    td = document.createElement('td');
    var deadline = formatDeadline(task.Deadline);
    td.append(document.createTextNode(deadline));
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

function selectProject(id) {
    if (id === selectedProjectId) return;

    var previouslySelectedProject = document.getElementsByClassName('selectedProject')[0];
    if (previouslySelectedProject !== undefined) {
        previouslySelectedProject.classList.remove('selectedProject');
    }

    var product = document.querySelector("[data-id='" + id + "']");
    product.classList.add('selectedProject');
    selectedProjectId = id;
    populateTaskView(id);
}

function selectTask(id) {
    if (id === selectedTaskId) return;

    var previouslySelectedTask = document.getElementsByClassName('selectedTask')[0];
    if (previouslySelectedTask !== undefined) {
        previouslySelectedTask.classList.remove('selectedTask');
    }

    var task = document.querySelector("[data-id='" + id + "']");
    task.classList.add('selectedTask');
    selectedTaskId = id;
    populateNote(id);
}



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


var selectedProjectId;
var selectedTaskId;

fillPriorityDropDown();
populateProjectList();
//showContextMenuWhenRightClickingOnTask();
