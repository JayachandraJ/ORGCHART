﻿var BasePath_ENV = "";
var HOST_ENV = window.location.protocol + '//' + window.location.host + "" + BasePath_ENV;

var ObjChange = [];
var ObjChangeType = [];
var Settings = {
    SelectShape: "RoundedRectangle",
    Skin: "white",
    ShowPicture: "Yes",
    SelectView: "Normal",
    SplitScreen: "Yes",
    SplitScreenDirection: "Vertical",
    TextColor: "black",
    UpArrow: "/Images/uparrow.jpg",
    DownArrow: "/Images/downarrow.jpg",
    BorderColor: "cyan",
    BorderWidth: 3,
    LineColor: "#634329",
    OrgChartType: "OD"
};

function calculateShapeXDimention(shape, viewType) {

    switch (shape) {
        case "RoundedRectangle":
            return 220;
            break;

        case "Circle":
            return 120;
            break;

        case "Ellipse":
            return 140;
            break;
    }
    return 160;
}

function calculateShapeYDimention(shape) {
    switch (shape) {
        case "RoundedRectangle":
            return 90;
            break;

        case "Circle":
            return 120;
            break;

        case "Ellipse":
            return 120;
            break;
    }
    return 80;
}

function init(w, h, SN) {
    // Set the Top Node and its decendent
    document.getElementById("mySavedModel").value = "{ \"class\": \"go.TreeModel\",  \"nodeDataArray\":" + GetNodeData(SN) + " }";

    var shape_X_Dimention = calculateShapeXDimention(Settings.SelectShape);
    var shape_Y_Dimention = calculateShapeYDimention(Settings.SelectShape);

    if (window.goSamples) goSamples();  // init for these samples -- you don't need to call this
    var $ = go.GraphObject.make;        // for conciseness in defining templates
    go.Diagram.inherit(SideTreeLayout, go.TreeLayout);

    myDiagram =
        $(go.Diagram, "myDiagramDiv", // must be the ID or reference to div
            {
                initialDocumentSpot: go.Spot.TopCenter,
                initialContentAlignment: go.Spot.TopCenter,
                maxSelectionCount: 1, // users can select only one part at a time
                layout:
                    $(SideTreeLayout,
                        {
                            treeStyle: go.TreeLayout.StyleLastParents,
                            arrangement: go.TreeLayout.ArrangementVertical,
                            // properties for most of the tree:
                            angle: 90,
                            layerSpacing: 20,
                            nodeSpacing: 20,
                            breadthLimit: w,

                            // properties for the "last parents":
                            alternateAngle: 90,
                            alternateLayerSpacing: 35,
                            alternateAlignment: go.TreeLayout.AlignmentBus,
                            alternateNodeSpacing: 20
                        }),
                "undoManager.isEnabled": true,  // enable undo & redo
                allowDragOut: false,
                allowDrop: false
            });

    myDiagram.addModelChangedListener(function (evt) {

        // ignore unimportant Transaction events
        if (!evt.isTransactionFinished) return;
        var txn = evt.object;  // a Transaction
        if (txn === null) return;
        //console.log(txn);
        // iterate over all of the actual ChangedEvents of the Transaction
        txn.changes.each(function (e) {

            // record node insertions and removals
            if (e.change === go.ChangedEvent.Property) {
                if (e.modelChange === "linkFromKey") {
                    console.log(evt.propertyName + " changed From key of link: " +
                        e.object + " from: " + e.oldValue + " to: " + e.newValue);
                } else if (e.modelChange === "linkToKey") {
                    console.log(evt.propertyName + " changed To key of link: " +
                        e.object + " from: " + e.oldValue + " to: " + e.newValue);
                }

            } else if (e.change === go.ChangedEvent.Insert && e.modelChange === "linkDataArray") {
                console.log(evt.propertyName + " added link: " + e.newValue);
            } else if (e.change === go.ChangedEvent.Remove && e.modelChange === "linkDataArray") {
                console.log(evt.propertyName + " removed link: " + e.oldValue);
            }
        });
    });

    myDiagram.doFocus = function () {
        var x = window.scrollX || window.pageXOffset;
        var y = window.scrollY || window.pageYOffset;
        go.Diagram.prototype.doFocus.call(this);
        window.scrollTo(x, y);
    }

    // when the document is modified, add a "*" to the title and enable the "Save" button
    myDiagram.addDiagramListener("Modified", function (e) {

        var button = document.getElementById("SaveButton");
        if (button) button.disabled = !myDiagram.isModified;
        var idx = document.title.indexOf("*");
        if (myDiagram.isModified) {
            if (idx < 0) document.title += "*";
        } else {
            if (idx >= 0) document.title = document.title.substr(0, idx);
        }
    });

    myDiagram.addDiagramListener("ViewportBoundsChanged", function (e) {
        let allowScroll = !e.diagram.viewportBounds.containsRect(e.diagram.documentBounds);
        myDiagram.allowHorizontalScroll = false;
        myDiagram.allowVerticalScroll = false;
    });

    // manage boss info manually when a node or link is deleted from the diagram
    myDiagram.addDiagramListener("SelectionDeleting", function (e) {
        var part = e.subject.first(); // e.subject is the myDiagram.selection collection,
        // so we'll get the first since we know we only have one selection
        myDiagram.startTransaction("clear boss");
        if (part instanceof go.Node) {
            var it = part.findTreeChildrenNodes(); // find all child nodes
            while (it.next()) { // now iterate through them and clear out the boss information
                var child = it.value;
                var bossText = child.findObject("boss"); // since the boss TextBlock is named, we can access it by name
                if (bossText === null) return;
                bossText.text = "";
            }
        } else if (part instanceof go.Link) {
            var child = part.toNode;
            var bossText = child.findObject("boss"); // since the boss TextBlock is named, we can access it by name
            if (bossText === null) return;
            bossText.text = "";
        }
        myDiagram.commitTransaction("clear boss");
    });

    myDiagram.addDiagramListener("InitialLayoutCompleted", function (e) {
        var dia = e.diagram;

        // add height for horizontal scrollbar
        dia.div.style.height = (dia.documentBounds.height + 24) + "px";
    });

    var levelColors = ["#FFFFFF/#FFFFFF", "#FFFFFF/#FFFFFF", "#FFFFFF/#FFFFFF", "#FFFFFF/#FFFFFF", "#FFFFFF/#FFFFFF", "#FFFFFF/#FFFFFF", "#FFFFFF/#FFFFFF", "#FFFFFF/#FFFFFF"];
    Settings.TextColor = "black";
    Settings.UpArrow = HOST_ENV + "/Content/Images/uparrow.jpg";
    Settings.DownArrow = HOST_ENV + "/Content/Images/downarrow.jpg";
    Settings.BorderWidth = 3;
    if (Settings.Skin.toUpperCase() == "BROWN") {
        levelColors = ["#634329/#634329", "#923222/#923222", "#e44c16/#e44c16", "#ec8026/#ec8026", "#fcaf17/#fcaf17", "#fed300/#fed300", "#f5eb07/#f5eb07", "#e44c16/#e44c16"];
        Settings.TextColor = "white";
        Settings.UpArrow = HOST_ENV + "/Content/Images/uparroww.png";
        Settings.DownArrow = HOST_ENV + "/Content/Images/downarrow.ico";
        //Settings.BorderColor = "cyan";
        Settings.BorderWidth = 0;
    }

    // override TreeLayout.commitNodes to also modify the background brush based on the tree depth level
    myDiagram.layout.commitNodes = function () {
        go.TreeLayout.prototype.commitNodes.call(myDiagram.layout);  // do the standard behavior
        // then go through all of the vertexes and set their corresponding node's Shape.fill
        // to a brush dependent on the TreeVertex.level value
        myDiagram.layout.network.vertexes.each(function (v) {
            if (v.node) {
                var level = v.level % (levelColors.length);
                var colors = levelColors[level].split("/");
                var shape = v.node.findObject("SHAPE");
                if (shape) shape.fill = $(go.Brush, "Linear", { 0: colors[0], 1: colors[1], start: go.Spot.Left, end: go.Spot.Right });
            }
        });
    };

    // This function is used to find a suitable ID when modifying/creating nodes.
    // We used the counter combined with findNodeDataForKey to ensure uniqueness.
    function getNextKey() {
        var key = nodeIdCounter;
        while (myDiagram.model.findNodeDataForKey(key) !== null) {
            key = nodeIdCounter--;
        }
        return key;
    }

    // Id first element but NOT CEO
    function isFirstElement(key) {

        if (key == document.getElementById("hdnOrgShowLevel").value && myDiagram.model.findNodeDataForKey(key).parent != "999999")
            return true;
        else
            return false;
    };

    // if not first element and has children then return true.
    function hasChildren(key) {

        // if first element 
        if (key == document.getElementById("hdnOrgShowLevel").value) return false;

        //if has children
        //alert(key + ":" + myDiagram.model.findNodeDataForKey(key).SOC_COUNT.toString() + ":" + (myDiagram.model.findNodeDataForKey(key).SOC_COUNT > 0).toString());
        return myDiagram.model.findNodeDataForKey(key).SOC_COUNT > 0 ? true : false;
    };

    var nodeIdCounter = -1; // use a sequence to guarantee key uniqueness as we add/remove/modify nodes

    // when a node is double-clicked
    function nodeDoubleClick(e, obj) {

        if (obj.data.parent != "999999") {
            var element = document.querySelector('.overlay');
            element.style.display = 'block';

            var URL = HOST_ENV + "/Version/ChangeShowLevel";
            var JsonData = {
                ShowLevel: obj.data.key,
                ParentLevel: obj.data.parent
            };

            jQuery.ajax({
                type: "POST",
                url: URL,
                data: JsonData,
                async: true,
                dateType: "json",
            }).done(function (JsonString) {
                if (JsonString.Message != "No Changes") {
                    document.getElementById("hdnOrgChartData").value = JsonString.ChartData;
                    document.getElementById("hdnOrgShowLevel").value = JsonString.UsedShowLevel;
                    loadJSON("{ \"class\": \"go.TreeModel\",  \"nodeDataArray\":" + JsonString.ChartData + " }");

                    myDiagram.startTransaction("properties");

                    var dia = e.diagram;

                    // add height for horizontal scrollbar
                    dia.div.style.height = (dia.documentBounds.height + 24) + "px";

                    myDiagram.commitTransaction("properties");

                    BreadGram();
                }
                element.style.display = 'none';
            });
        }
    }

    // this is used to determine feedback during drags
    function mayWorkFor(node1, node2) {
        if (!(node1 instanceof go.Node)) return false;  // must be a Node
        if (node1 === node2) return false;  // cannot work for yourself
        if (node2.isInTreeOf(node1)) return false;  // cannot work for someone who works for you
        return true;
    }

    // This function provides a common style for most of the TextBlocks.
    // Some of these values may be overridden in a particular TextBlock.
    function textStyle() {
        return { font: "9pt  Segoe UI,sans-serif", stroke: Settings.TextColor };
    }

    // This converter is used by the Picture.
    function findHeadShot(key) {
        if (key < 0 || key > 100000006) return "images/HSnopic.png"; // There are only 16 images on the server
        var currentdate = new Date();
        var datetime = currentdate.getDate() + "/" + (currentdate.getMonth() + 1) + "/" + currentdate.getFullYear() +
            " @ " + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();

        return HOST_ENV + "/Content/Images/PHOTOS/" + key + ".jpg?" + datetime;
    }

    function SetParentChildRelationship(e, SelectNodeObj) {
        var element = document.querySelector('.overlay');
        element.style.display = 'block';

        ObjChangeType = [];
        ObjChange = [];

        var Obj = myDiagram.findNodeForKey(SelectNodeObj.parent);
        SelectNodeObj.SUP_DISPLAY_NAME = Obj.data.FULL_NAME;
        alert(SelectNodeObj.SUP_DISPLAY_NAME + ":" + Obj.data.FULL_NAME);
        ObjChange.push(SelectNodeObj);
        ObjChangeType.push("C");

        var JsonData = {
            VersionData: JSON.stringify(ObjChange),
            ChangeType: ObjChangeType,
            OperType: document.getElementById("hdnOrgType").value
        };

        jQuery.ajax({
            type: "POST",
            url: HOST_ENV + "/Version/SaveVersion",
            data: JsonData,
            async: true,
            dateType: "json",
        }).done(function (Json) {
            if (Json.Success == "Yes" || Json.Success == "No") {
                document.getElementById("hdnOrgChartData").value = Json.ChartData;
                document.getElementById("hdnOrgShowLevel").value = Json.UsedShowLevel;
                document.getElementById("hdnOrgVersion").value = Json.UsedVersion;
                loadJSON("{ \"class\": \"go.TreeModel\",  \"nodeDataArray\":" + Json.ChartData + " }");

                myDiagram.startTransaction("properties");

                var dia = e.diagram;

                // add height for horizontal scrollbar
                dia.div.style.height = (dia.documentBounds.height + 24) + "px";

                myDiagram.commitTransaction("properties");

                if (Json.Success == "No") alert(Json.Message);
            }
            element.style.display = 'none';
        });
    }

    // This converts dotted line to a different shape
    function findLinkColorForLevel(key) {
        return Settings.LineColor;
    };

    function addCommas(nStr) {
        nStr += '';
        var x = nStr.split('.');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }

    myDiagram.nodeTemplate =
    $(go.Node, "Auto",
        { doubleClick: nodeDoubleClick },
        { // handle dragging a Node onto a Node to (maybe) change the reporting relationship
            mouseDragEnter: function (e, node, prev) {
                var diagram = node.diagram;
                var selnode = diagram.selection.first();
                if (!mayWorkFor(selnode, node)) return;
                var shape = node.findObject("SHAPE");
                if (shape) {
                    shape._prevFill = shape.fill;  // remember the original brush
                    shape.fill = "darkred";
                }
            },
            mouseDragLeave: function (e, node, next) {
                var shape = node.findObject("SHAPE");
                if (shape && shape._prevFill) {
                    shape.fill = shape._prevFill;  // restore the original brush
                }
            },
            mouseDrop: function (e, node) {
                var diagram = node.diagram;
                var selnode = diagram.selection.first();  // assume just one Node in selection
                if (mayWorkFor(selnode, node)) {
                    // find any existing link into the selected node
                    var link = selnode.findTreeParentLink();
                    if (link !== null) {  // reconnect any existing link
                        link.fromNode = node;
                    } else {  // else create a new link
                        diagram.toolManager.linkingTool.insertLink(node, node.port, selnode, selnode.port);
                    }
                    SetParentChildRelationship(e, selnode.data);
                }
            }
        },
        // for sorting, have the Node.text be the data.name
        new go.Binding("text", "name"),
        // bind the Part.layerName to control the Node's layer depending on whether it isSelected
        new go.Binding("layerName", "isSelected", function (sel) { return sel ? "Foreground" : ""; }).ofObject(),
        // define the node's outer shape
        $(go.Shape, Settings.SelectShape,
            {
                name: "SHAPE", fill: "white", strokeWidth: Settings.BorderWidth, stroke: Settings.BorderColor,
                // set the port properties:
                portId: "", fromLinkable: false, toLinkable: false, cursor: "pointer"
            }),

        $(go.Panel, "Vertical",
            $(go.Picture,
                {
                    name: 'UpArrow',
                    desiredSize: new go.Size(25, 15),
                    source: Settings.UpArrow,
                    visible: false
                },
                new go.Binding("visible", "key", isFirstElement)),

            $(go.Panel, "Horizontal",
                $(go.Picture,
                    {
                        name: "Picture",
                        desiredSize: new go.Size(50, 50),
                        margin: new go.Margin(4, 4, 4, 4),
                    },
                    new go.Binding("source", "key", findHeadShot)),

                // define the panel where the text will appear
                $(go.Panel, "Table",
                    {
                        desiredSize: new go.Size(shape_X_Dimention, shape_Y_Dimention),
                        margin: new go.Margin(4, 4, 4, 4),
                        defaultAlignment: go.Spot.Left
                    },
                    $(go.RowColumnDefinition,
                        { row: 2, separatorStrokeWidth: 1, separatorStroke: "orange", separatorPadding: 6 }),
                    $(go.TextBlock, textStyle(),  // the name
                        {
                            row: 0, column: 0, columnSpan: 5,
                            font: "12pt Segoe UI,sans-serif",
                            editable: true, isMultiline: false,
                            minSize: new go.Size(10, 16), stroke: Settings.TextColor
                        },
                        new go.Binding("text", "name").makeTwoWay()),
                    $(go.TextBlock, textStyle(),
                        {
                            row: 1, column: 0, columnSpan: 5,
                            editable: true, isMultiline: false,
                            minSize: new go.Size(10, 14),
                            margin: new go.Margin(0, 0, 0, 3), stroke: Settings.TextColor
                        },
                        new go.Binding("text", "title").makeTwoWay()),
                    $(go.TextBlock, textStyle(),
                        { row: 2, column: 0, stroke: Settings.TextColor },
                        new go.Binding("text", "key", function (v) { return v; })),
                    $(go.TextBlock, textStyle(),
                        { name: "boss", row: 2, column: 3, stroke: Settings.TextColor }, // we include a name so we can access this TextBlock when deleting Nodes/Links
                        new go.Binding("text", "parent", function (v) { return v; }))
                )  // end Table Panel
            ), // end Horizontal Panel

            $(go.Picture,
                {
                    name: 'DownArrow',
                    desiredSize: new go.Size(25, 15),
                    source: Settings.DownArrow,
                    alignment: go.Spot.Bottom,
                    visible: false
                },
                new go.Binding("visible", "key", hasChildren))

        ), // end Vertical Panel
        {
            selectionAdornmentTemplate:
                $(go.Adornment, "Auto",
                    $(go.Shape, "RoundedRectangle",
                        { fill: null, stroke: "dodgerblue", strokeWidth: 0 }),
                    $(go.Placeholder)
                )  // end Adornment
        }
    );  // end Node
   
    // Assume that the SideTreeLayout determines whether a node is an "assistant" if a particular data property is true.
    // You can adapt this code to decide according to your app's needs.
    function isAssistant(n) {
        if (n === null) return false;
        return n.data.isAssistant;
    }


    // This is a custom TreeLayout that knows about "assistants".
    // A Node for which isAssistant(n) is true will be placed at the side below the parent node
    // but above all of the other child nodes.
    // An assistant node may be the root of its own subtree.
    // An assistant node may have its own assistant nodes.
    function SideTreeLayout() {
        go.TreeLayout.call(this);
    }

    SideTreeLayout.prototype.makeNetwork = function (coll) {
        var net = go.TreeLayout.prototype.makeNetwork.call(this, coll);
        // copy the collection of TreeVertexes, because we will modify the network
        var vertexcoll = new go.Set(/*go.TreeVertex*/);
        vertexcoll.addAll(net.vertexes);
        for (var it = vertexcoll.iterator; it.next();) {
            var parent = it.value;
            // count the number of assistants
            var acount = 0;
            var ait = parent.destinationVertexes;
            while (ait.next()) {
                if (isAssistant(ait.value.node)) acount++;
            }
            // if a vertex has some number of children that should be assistants
            if (acount > 0) {
                // remember the assistant edges and the regular child edges
                var asstedges = new go.Set(/*go.TreeEdge*/);
                var childedges = new go.Set(/*go.TreeEdge*/);
                var eit = parent.destinationEdges;
                while (eit.next()) {
                    var e = eit.value;
                    if (isAssistant(e.toVertex.node)) {
                        asstedges.add(e);
                    } else {
                        childedges.add(e);
                    }
                }
                // first remove all edges from PARENT
                eit = asstedges.iterator;
                while (eit.next()) { parent.deleteDestinationEdge(eit.value); }
                eit = childedges.iterator;
                while (eit.next()) { parent.deleteDestinationEdge(eit.value); }
                // if the number of assistants is odd, add a dummy assistant, to make the count even
                if (acount % 2 == 1) {
                    var dummy = net.createVertex();
                    net.addVertex(dummy);
                    net.linkVertexes(parent, dummy, asstedges.first().link);
                }
                // now PARENT should get all of the assistant children
                eit = asstedges.iterator;
                while (eit.next()) {
                    parent.addDestinationEdge(eit.value);
                }
                // create substitute vertex to be new parent of all regular children
                var subst = net.createVertex();
                net.addVertex(subst);
                // reparent regular children to the new substitute vertex
                eit = childedges.iterator;
                while (eit.next()) {
                    var ce = eit.value;
                    ce.fromVertex = subst;
                    subst.addDestinationEdge(ce);
                }
                // finally can add substitute vertex as the final odd child,
                // to be positioned at the end of the PARENT's immediate subtree.
                var newedge = net.linkVertexes(parent, subst, null);
            }
        }
        return net;
    };

    SideTreeLayout.prototype.assignTreeVertexValues = function (v) {
        // if a vertex has any assistants, use Bus alignment
        var any = false;
        var children = v.children;
        for (var i = 0; i < children.length; i++) {
            var c = children[i];
            if (isAssistant(c.node)) {
                any = true;
                break;
            }
        }
        if (any) {
            // this is the parent for the assistant(s)
            v.alignment = go.TreeLayout.AlignmentBus;  // this is required
            v.nodeSpacing = 50; // control the distance of the assistants from the parent's main links
        } else if (v.node == null && v.childrenCount > 0) {
            // found the substitute parent for non-assistant children
            //v.alignment = go.TreeLayout.AlignmentCenterChildren;
            //v.breadthLimit = 3000;
            v.layerSpacing = 0;
        }
    };

    SideTreeLayout.prototype.commitLinks = function () {
        go.TreeLayout.prototype.commitLinks.call(this);
        // make sure the middle segment of an orthogonal link does not cross over the assistant subtree
        var eit = this.network.edges.iterator;
        while (eit.next()) {
            var e = eit.value;
            if (e.link == null) continue;
            var r = e.link;
            // does this edge come from a substitute parent vertex?
            var subst = e.fromVertex;
            if (subst.node == null && r.routing == go.Link.Orthogonal) {
                r.updateRoute();
                r.startRoute();
                // middle segment goes from point 2 to point 3
                var p1 = subst.center;  // assume artificial vertex has zero size
                var p2 = r.getPoint(2).copy();
                var p3 = r.getPoint(3).copy();
                var p5 = r.getPoint(r.pointsCount - 1);
                var dist = 10;
                if (subst.angle == 270 || subst.angle == 180) dist = -20;
                if (subst.angle == 90 || subst.angle == 270) {
                    p2.y = p5.y - dist; // (p1.y+p5.y)/2;
                    p3.y = p5.y - dist; // (p1.y+p5.y)/2;
                } else {
                    p2.x = p5.x - dist; // (p1.x+p5.x)/2;
                    p3.x = p5.x - dist; // (p1.x+p5.x)/2;
                }
                r.setPoint(2, p2);
                r.setPoint(3, p3);
                r.commitRoute();
            }
        }
    };  // end of SideTreeLayout

    // the context menu allows users to make a position vacant,
    // remove a role and reassign the subtree, or remove a department
    myDiagram.nodeTemplate.contextMenu =
        $(go.Adornment, "Vertical",
            $("ContextMenuButton",
                $(go.TextBlock, "New Position"),
                {
                    click: function (e, obj) {
                        var node = obj.part.adornedPart;
                        if (node !== null) {
                            var thisemp = node.data;
                            AddNewPositionModal(thisemp.key);
                        }
                    }
                }
            ),
            $("ContextMenuButton",
                $(go.TextBlock, "Add Picture"),
                {
                    click: function (e, obj) {
                        var node = obj.part.adornedPart;
                        if (node !== null) {
                            var thisemp = node.data;
                            AddNewImage(thisemp.key, thisemp.LEVEL_ID);

                        }
                    }
                }
            ),
            $("ContextMenuButton",
                $(go.TextBlock, "Check/UnCheck Assistance"),
                {
                    click: function (e, obj) {
                        var node = obj.part.adornedPart;
                        if (node !== null) {
                            var thisemp = node.data;
                            CheckUnCheckAssistance(thisemp.LEVEL_ID, thisemp.PARENT_LEVEL_ID);

                        }
                    }
                }
            ),
            $("ContextMenuButton",
                $(go.TextBlock, "Sorting"),
                {
                    click: function (e, obj) {
                        var node = obj.part.adornedPart;
                        if (node !== null) {
                            var thisemp = node.data;
                            ChangeSortOrder(thisemp.LEVEL_ID);
                        }
                    }
                }
            )
        );

    // define the Link template
    myDiagram.linkTemplate =
        $(go.Link, go.Link.AvoidsNodes,  // may be either Orthogonal or AvoidsNodes
            { corner: 5, relinkableFrom: false, relinkableTo: false },
            $(go.Shape,
                //{ stroke: "#00a4a4" },
                new go.Binding("stroke", "LEVEL_NO", findLinkColorForLevel),
                { strokeWidth: 2 }
            ));  // the link shape

    // read in the JSON-format data from the "mySavedModel" element
    load();


    // support editing the properties of the selected person in HTML
    if (window.Inspector) myInspector = new Inspector("myInspector", myDiagram,
        {
            properties: {
                "key": { readOnly: true },
                "comments": {}
            }
        });
}

// Show the diagram's model in JSON format
function save() {
    document.getElementById("mySavedModel").value = myDiagram.model.toJson();
    myDiagram.isModified = false;
}

function load() {
    myDiagram.model = go.Model.fromJson(document.getElementById("mySavedModel").value);
}

// Show the diagram's model in JSON format
function loadJSON(strJson) {
    if (strJson) {
        document.getElementById("mySavedModel").value = strJson;
    }
    else document.getElementById("mySavedModel").value = "{ \"class\": \"go.TreeModel\",  \"nodeDataArray\":\"\" }";

    myDiagram.model = go.Model.fromJson(document.getElementById("mySavedModel").value);
}