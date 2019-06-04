var NodeData = [
    { "key": 1, "name": "Stella Payne Diaz", "title": "CEO" },
    { "key": 2, "name": "Luke Warm", "title": "VP Marketing/Sales", "parent": 1 },
    { "key": 3, "name": "Meg Meehan Hoffa", "title": "Sales", "parent": 2 },
    { "key": 4, "name": "Peggy Flaming", "title": "VP Engineering", "parent": 1 },
    { "key": 5, "name": "Saul Wellingood", "title": "Manufacturing", "parent": 4 },
    { "key": 6, "name": "Al Ligori", "title": "Marketing", "parent": 2 },
    { "key": 7, "name": "Dot Stubadd", "title": "Sales Rep", "parent": 3 },
    { "key": 8, "name": "Les Ismore", "title": "Project Mgr", "parent": 5 },
    { "key": 9, "name": "April Lynn Parris", "title": "Events Mgr", "parent": 6 },
    { "key": 10, "name": "Xavier Breath", "title": "Engineering", "parent": 4 },
    { "key": 11, "name": "Anita Hammer", "title": "Process", "parent": 5 },
    { "key": 12, "name": "Billy Aiken", "title": "Software", "parent": 10 },
    { "key": 13, "name": "Stan Wellback", "title": "Testing", "parent": 10 },
    { "key": 14, "name": "Marge Innovera", "title": "Hardware", "parent": 10 },
    { "key": 15, "name": "Evan Elpus", "title": "Quality", "parent": 5 },
    { "key": 16, "name": "Lotta B. Essen", "title": "Sales Rep", "parent": 3 }
];

var SelectedNodeData = [], SelectedNodeKey = [];
function GetNodeData(NodeNo) {

    SelectedNodeData = [];
    SelectedNodeKey = [];

    $.each(NodeData, function (key, item) {
        if (NodeNo == item.key) {
            SelectedNodeData.push({
                key: item.key,
                name: item.name,
                title: item.title
            });
            SelectedNodeKey.push({ topnode: item.key });
        }
    });

    $.each(SelectedNodeKey, function (key, itemnode) {
        $.each(NodeData, function (key, itemvalue) {
            if (itemvalue.parent == itemnode.topnode) {
                SelectedNodeData.push({
                    key: itemvalue.key,
                    name: itemvalue.name,
                    title: itemvalue.title,
                    parent: itemvalue.parent
                });
                SelectedNodeKey.push({ topnode: itemvalue.key });
            }
        });
    });

    return JSON.stringify(SelectedNodeData);
}

function GetOrgTitleKey(NodeTitle) {
    var KeyValue = "0";
    $.each(NodeData, function (key, item) {
        if (NodeTitle.toUpperCase().trim() == item.title.toUpperCase().trim()) {
            KeyValue = item.key.toString().toUpperCase().trim();
            return item.key.toString();
        }
    });
    if (KeyValue != "0") return KeyValue;

    return 0;
}
