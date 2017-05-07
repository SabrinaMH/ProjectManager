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

get('api/project')
    .then((projects) => {
        var projectList = [];
        projects.forEach(function (project) { projectList.push(project.Title) });
        populateProjectList(projectList);
    })
    .catch((err) => {
        console.log(err);
    });

function populateProjectList(list) {
    var ul = $('<ul>').appendTo($('#projectList'));
    list.forEach(function (title, index) {
        ul.append(
            $(document.createElement('li')).text(title)
        );
    });
}