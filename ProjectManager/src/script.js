
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

function getProject(id) {
    get('project/' + id)
        .then((viewModel) => {
            appendToProjectList(viewModel);
        })
        .catch((err) => {
            console.log(err);
        });
}

function getProjectList() {
    get('project')
        .then((viewModels) => {
            var projectList = [];
            viewModels.forEach(function(viewModel) { projectList.push(viewModel.Title) });
            populateProjectList(projectList);
        })
        .catch((err) => {
            console.log(err);
        });
}

function populateProjectList(list) {
    var ul = $('<ul>').appendTo($('#projectList'));
    list.forEach(function (title, index) {
        ul.append(
            $(document.createElement('li')).append(
                $(document.createElement('a')).text(title).attr('href', 'project')
            )
        );
    });
}

function appendToProjectList(viewModel) {
    var ul = $('#projectList ul');
    ul.append(
        $(document.createElement('li')).text(viewModel.Title)
    );
}

$('#addProjectForm').submit(function (e) {
    var title = $("#title").val().toString();
    var deadline = $("#deadline").val();
    var formData = { Title: title, Deadline: deadline }
    var formURL = $(this).attr("action");
    $.ajax(
        {
            url: formURL,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            success: function (projectId, textStatus, jqXHR) {
                getProject(projectId);
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

getProjectList();