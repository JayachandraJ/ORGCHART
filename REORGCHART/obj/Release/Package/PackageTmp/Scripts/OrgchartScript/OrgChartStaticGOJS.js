function initOrgChartStatic(JsonData, w, h) {

    var shape_X_Dimention = calculateShapeXDimention(Settings.SelectShape);
    var shape_Y_Dimention = calculateShapeYDimention(Settings.SelectShape);

    if (window.goSamples) goSamples();  // init for these samples -- you don't need to call this
    var $ = go.GraphObject.make;  // for conciseness in defining templates
    go.Diagram.inherit(SideTreeLayout, go.TreeLayout);

    myDiagram =
        $(go.Diagram, "myDiagramDiv",  // the DIV HTML element
            {
                // Put the diagram contents at the top center of the viewport
                initialDocumentSpot: go.Spot.TopCenter,
                initialViewportSpot: go.Spot.TopCenter,
                // OR: Scroll to show a particular node, once the layout has determined where that node is
                //"InitialLayoutCompleted": function(e) {
                //  var node = e.diagram.findNodeForKey(28);
                //  if (node !== null) e.diagram.commandHandler.scrollToPart(node);
                //},
                layout:
                    $(SideTreeLayout,  // use a TreeLayout to position all of the nodes
                        {
                            treeStyle: go.TreeLayout.StyleLastParents,
                            // properties for most of the tree:
                            angle: 90,
                            layerSpacing: 80,
                            // properties for the "last parents":
                            alternateAngle: 0,
                            alternateAlignment: go.TreeLayout.AlignmentStart,
                            alternateNodeIndent: 20,
                            alternateNodeIndentPastParent: 1,
                            alternateNodeSpacing: 20,
                            alternateLayerSpacing: 40,
                            alternateLayerSpacingParentOverlap: 1,
                            alternatePortSpot: new go.Spot(0.001, 1, 20, 0),
                            alternateChildPortSpot: go.Spot.Left
                        }),
                "undoManager.isEnabled": true,  // enable undo & redo
                allowDragOut: true,
                allowDrop: true 
            });

    myDiagram.addDiagramListener("InitialLayoutCompleted", function (e) {
        var dia = e.diagram;

        // add height for horizontal scrollbar
        var ShowHeight = h;
        if (dia.documentBounds.height + 24 >= h) ShowHeight = dia.documentBounds.height + 24;
        dia.div.style.height = ShowHeight + "px";
    });

    myDiagram.doFocus = function () {
        var x = window.scrollX || window.pageXOffset;
        var y = window.scrollY || window.pageYOffset;
        go.Diagram.prototype.doFocus.call(this);
        window.scrollTo(x, y);
    }

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

    if (document.getElementById("hdnOrgView").value == "Normal") {

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
                                { row: 3, separatorStrokeWidth: 1, separatorStroke: "orange", separatorPadding: 6 }),
                            $(go.TextBlock, textStyle(),  // the name
                                {
                                    row: 0, column: 0, columnSpan: 5,
                                    font: "12pt Segoe UI,sans-serif",
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 16), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "FULL_NAME").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 1, column: 0, columnSpan: 5,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    margin: new go.Margin(0, 0, 0, 3), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "POSITION_TITLE").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                { row: 2, column: 0, stroke: Settings.TextColor },
                                new go.Binding("text", "key", function (v) { return v; })),
                            $(go.TextBlock, textStyle(),
                                { name: "boss", row: 2, column: 3, stroke: Settings.TextColor }, // we include a name so we can access this TextBlock when deleting Nodes/Links
                                new go.Binding("text", "parent", function (v) { return v; })),
                            $(go.TextBlock, textStyle(),
                                { row: 3, column: 0, stroke: Settings.TextColor },
                                new go.Binding("text", "SOC_COUNT", function (v) { return "SOC: " + v; })),
                            $(go.TextBlock, textStyle(),
                                { name: "boss", row: 3, column: 3, stroke: Settings.TextColor }, // we include a name so we can access this TextBlock when deleting Nodes/Links
                                new go.Binding("text", "NOR_COUNT", function (v) { return "NOR: " + v; })),
                            $(go.TextBlock, textStyle(),  // the comments
                                {
                                    row: 4, column: 0, columnSpan: 5,
                                    font: "italic 9pt sans-serif",
                                    wrap: go.TextBlock.WrapFit,
                                    editable: true,  // by default newlines are allowed
                                    minSize: new go.Size(10, 14), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "comments").makeTwoWay())
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
    }
    else if (document.getElementById("hdnOrgView").value == "Mcbitss") {

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
                                { row: 1, separatorStrokeWidth: 1, separatorStroke: "orange", separatorPadding: 2 }),
                            $(go.TextBlock, textStyle(),  // the name
                                {
                                    row: 0, column: 0, columnSpan: 5,
                                    font: "12pt Segoe UI,sans-serif",
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 16), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "FULL_NAME").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 1, column: 0, columnSpan: 5,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    margin: new go.Margin(0, 0, 0, 3), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "POSITION_TITLE").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 2, column: 0, columnSpan: 5,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    margin: new go.Margin(0, 0, 0, 3), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "AD_DEPARTMENT").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 3, column: 0, columnSpan: 5,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    margin: new go.Margin(0, 0, 0, 3), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "DIVISION").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                { row: 4, column: 0, margin: new go.Margin(0, 0, 0, 3), stroke: Settings.TextColor },
                                new go.Binding("text", "key", function (v) { return "Corporate No: " + v; })),
                            $(go.TextBlock, textStyle(),
                                { name: "boss", row: 4, column: 3, margin: new go.Margin(0, 0, 0, 3), stroke: Settings.TextColor }, // we include a name so we can access this TextBlock when deleting Nodes/Links
                                new go.Binding("text", "SOC_COUNT", function (v) { return "SOC :" + v; }))

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
    }
    else if (document.getElementById("hdnOrgView").value == "Cost") {
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
                        }

                        SetParentChildRelationship(e, selnode.data);
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
                                { row: 3, separatorStrokeWidth: 1, separatorStroke: "orange", separatorPadding: 6 }),
                            $(go.TextBlock, textStyle(),  // the name
                                {
                                    row: 0, column: 0, columnSpan: 5,
                                    font: "12pt Segoe UI,sans-serif",
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 16), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "FULL_NAME").makeTwoWay()),
                            $(go.TextBlock, "Title: ", textStyle(),
                                { row: 1, column: 0, stroke: Settings.TextColor }),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 1, column: 1, columnSpan: 4,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    margin: new go.Margin(0, 0, 0, 3), stroke: Settings.TextColor
                                },
                                new go.Binding("text", "POSITION_TITLE").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                { row: 2, column: 0, stroke: Settings.TextColor },
                                new go.Binding("text", "POSITION_COST", function (v) { return "Cost: " + addCommas(parseFloat(v).toFixed(2).toLocaleString()); })),
                            $(go.TextBlock, textStyle(),
                                { name: "boss", row: 2, column: 4, stroke: Settings.TextColor, }, // we include a name so we can access this TextBlock when deleting Nodes/Links
                                new go.Binding("text", "POSITION_CALCULATED_COST", function (v) { return "Total Cost: " + addCommas(parseFloat(v).toFixed(2).toLocaleString()); })),
                            $(go.TextBlock, textStyle(),
                                { row: 3, column: 0, stroke: Settings.TextColor },
                                new go.Binding("text", "SOC_COUNT", function (v) { return "SOC: " + v; })),
                            $(go.TextBlock, textStyle(),
                                { name: "boss", row: 3, column: 4, stroke: Settings.TextColor }, // we include a name so we can access this TextBlock when deleting Nodes/Links
                                new go.Binding("text", "NOR_COUNT", function (v) { return "NOR: " + v; }))
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
                ),
                {
                    selectionAdornmentTemplate:
                        $(go.Adornment, "Auto",
                            $(go.Shape, "RoundedRectangle",
                                { fill: null, stroke: "dodgerblue", strokeWidth: 0 }),
                            $(go.Placeholder)
                        )  // end Adornment
                }  // end Vertical Panel
            );  // end Node
    }
    else if (document.getElementById("hdnOrgView").value == "UNHCR") {

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
                        // define the panel where the text will appear
                        $(go.Panel, "Table",
                            {
                                desiredSize: new go.Size(shape_X_Dimention, shape_Y_Dimention),
                                margin: new go.Margin(4, 4, 4, 4),
                                defaultAlignment: go.Spot.Left
                            },
                            $(go.RowColumnDefinition,
                                { row: 1, separatorStrokeWidth: 1, separatorStroke: "orange", separatorPadding: 2 }),
                            $(go.TextBlock, textStyle(),  // the name
                                {
                                    row: 0, column: 0, columnSpan: 6,
                                    font: "12pt Segoe UI,sans-serif",
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 16),
                                    stroke: Settings.TextColor,
                                    alignment: go.Spot.TopCenter
                                },
                                new go.Binding("text", "FULL_NAME").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 1, column: 0, columnSpan: 6,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    margin: new go.Margin(0, 0, 0, 3),
                                    stroke: Settings.TextColor,
                                    alignment: go.Spot.TopCenter
                                },
                                new go.Binding("text", "POSITION_NUMBER").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 2, column: 0, columnSpan: 6,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    margin: new go.Margin(0, 0, 0, 3),
                                    stroke: Settings.TextColor,
                                    alignment: go.Spot.TopCenter
                                },
                                new go.Binding("text", "TITLE").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 3, column: 0, columnSpan: 6,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    margin: new go.Margin(0, 0, 0, 3),
                                    stroke: Settings.TextColor,
                                    alignment: go.Spot.TopCenter
                                },
                                new go.Binding("text", "GRADE").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 4, column: 0, columnSpan: 4,
                                    margin: new go.Margin(0, 0, 0, 3),
                                    stroke: Settings.TextColor,
                                    alignment: go.Spot.Left
                                },
                                new go.Binding("text", "NOR_COUNT", function (v) { return "NOR : " + v; })),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 4, column: 3, columnSpan: 4,
                                    margin: new go.Margin(0, 0, 0, 3),
                                    stroke: Settings.TextColor,
                                    alignment: go.Spot.Right
                                },
                                new go.Binding("text", "SOC_COUNT", function (v) { return "SOC :" + v; }))
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
    }
    else if (document.getElementById("hdnOrgView").value == "UNHCR_COST") {
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
                        }

                        SetParentChildRelationship(e, selnode.data);
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
                        // define the panel where the text will appear
                        $(go.Panel, "Table",
                            {
                                desiredSize: new go.Size(shape_X_Dimention, shape_Y_Dimention),
                                margin: new go.Margin(4, 4, 4, 4),
                                defaultAlignment: go.Spot.TopCenter
                            },
                            $(go.RowColumnDefinition,
                                { row: 3, separatorStrokeWidth: 1, separatorStroke: "orange", separatorPadding: 6 }),
                            $(go.TextBlock, textStyle(),  // the name
                                {
                                    row: 0, column: 0, columnSpan: 6,
                                    font: "12pt Segoe UI,sans-serif",
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 16),
                                    stroke: Settings.TextColor,
                                    textAlign: "center"
                                },
                                new go.Binding("text", "FULL_NAME").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 1, column: 0, columnSpan: 6,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    stroke: Settings.TextColor,
                                    alignment: go.Spot.TopCenter
                                },
                                new go.Binding("text", "TITLE").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 2, column: 0, columnSpan: 6,
                                    editable: true, isMultiline: false,
                                    minSize: new go.Size(10, 14),
                                    stroke: Settings.TextColor,
                                    alignment: go.Spot.TopCenter
                                },
                                new go.Binding("text", "GRADE").makeTwoWay()),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 3, column: 0, columnSpan: 4,
                                    alignment: go.Spot.Left,
                                    stroke: Settings.TextColor
                                },
                                new go.Binding("text", "BUDGET_SALARY", function (v) { return "" + addCommas(parseFloat(v).toFixed(2).toLocaleString()); })),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 3, column: 4, columnSpan: 3,
                                    alignment: go.Spot.Right,
                                    stroke: Settings.TextColor,
                                }, // we include a name so we can access this TextBlock when deleting Nodes/Links
                                new go.Binding("text", "POSITION_CALCULATED_COST", function (v) { return "" + addCommas(parseFloat(v).toFixed(2).toLocaleString()); })),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 4, column: 0, columnSpan: 4,
                                    alignment: go.Spot.Left,
                                    stroke: Settings.TextColor
                                },
                                new go.Binding("text", "SOC_COUNT", function (v) { return "SOC: " + v; })),
                            $(go.TextBlock, textStyle(),
                                {
                                    row: 4, column: 4, columnSpan: 3,
                                    alignment: go.Spot.Right,
                                    stroke: Settings.TextColor
                                },
                                new go.Binding("text", "NOR_COUNT", function (v) { return "NOR: " + v; }))
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
                ),
                {
                    selectionAdornmentTemplate:
                        $(go.Adornment, "Auto",
                            $(go.Shape, "RoundedRectangle",
                                { fill: null, stroke: "dodgerblue", strokeWidth: 0 }),
                            $(go.Placeholder)
                        )  // end Adornment
                }  // end Vertical Panel
            );  // end Node
    }

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

    // define the Link template, a simple orthogonal line
    myDiagram.linkTemplate =
        $(go.Link, go.Link.Orthogonal,
            { corner: 5, selectable: false },
        $(go.Shape, { strokeWidth: 3, stroke: Settings.LineColor }));  // dark gray, rounded corner links

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

    // set up the nodeDataArray, describing each person/position
    var nodeDataArray = [
        { key: 0, name: "Ban Ki-moon 반기문", nation: "South Korea", title: "Secretary-General of the United Nations", headOf: "Secretariat" },
        { key: 1, boss: 0, name: "Patricia O'Brien", nation: "Ireland", title: "Under-Secretary-General for Legal Affairs and United Nations Legal Counsel", headOf: "Office of Legal Affairs" },
        { key: 3, boss: 1, name: "Peter Taksøe-Jensen", nation: "Denmark", title: "Assistant Secretary-General for Legal Affairs" },
        { key: 9, boss: 3, name: "Other Employees" },
        { key: 4, boss: 1, name: "Maria R. Vicien - Milburn", nation: "Argentina", title: "General Legal Division Director", headOf: "General Legal Division" },
        { key: 10, boss: 4, name: "Other Employees" },
        { key: 5, boss: 1, name: "Václav Mikulka", nation: "Czech Republic", title: "Codification Division Director", headOf: "Codification Division" },
        { key: 11, boss: 5, name: "Other Employees" },
        { key: 6, boss: 1, name: "Sergei Tarassenko", nation: "Russia", title: "Division for Ocean Affairs and the Law of the Sea Director", headOf: "Division for Ocean Affairs and the Law of the Sea" },
        { key: 12, boss: 6, name: "Alexandre Tagore Medeiros de Albuquerque", nation: "Brazil", title: "Chairman of the Commission on the Limits of the Continental Shelf", headOf: "The Commission on the Limits of the Continental Shelf" },
        { key: 17, boss: 12, name: "Peter F. Croker", nation: "Ireland", title: "Chairman of the Committee on Confidentiality", headOf: "The Committee on Confidentiality" },
        { key: 31, boss: 17, name: "Michael Anselme Marc Rosette", nation: "Seychelles", title: "Vice Chairman of the Committee on Confidentiality" },
        { key: 32, boss: 17, name: "Kensaku Tamaki", nation: "Japan", title: "Vice Chairman of the Committee on Confidentiality" },
        { key: 33, boss: 17, name: "Osvaldo Pedro Astiz", nation: "Argentina", title: "Member of the Committee on Confidentiality" },
        { key: 34, boss: 17, name: "Yuri Borisovitch Kazmin", nation: "Russia", title: "Member of the Committee on Confidentiality" },
        { key: 18, boss: 12, name: "Philip Alexander Symonds", nation: "Australia", title: "Chairman of the Committee on provision of scientific and technical advice to coastal States", headOf: "Committee on provision of scientific and technical advice to coastal States" },
        { key: 35, boss: 18, name: "Emmanuel Kalngui", nation: "Cameroon", title: "Vice Chairman of the Committee on provision of scientific and technical advice to coastal States" },
        { key: 36, boss: 18, name: "Sivaramakrishnan Rajan", nation: "India", title: "Vice Chairman of the Committee on provision of scientific and technical advice to coastal States" },
        { key: 37, boss: 18, name: "Francis L. Charles", nation: "Trinidad and Tobago", title: "Member of the Committee on provision of scientific and technical advice to costal States" },
        { key: 38, boss: 18, name: "Mihai Silviu German", nation: "Romania", title: "Member of the Committee on provision of scientific and technical advice to costal States" },
        { key: 19, boss: 12, name: "Lawrence Folajimi Awosika", nation: "Nigeria", title: "Vice Chairman of the Commission on the Limits of the Continental Shelf" },
        { key: 20, boss: 12, name: "Harald Brekke", nation: "Norway", title: "Vice Chairman of the Commission on the Limits of the Continental Shelf" },
        { key: 21, boss: 12, name: "Yong-Ahn Park", nation: "South Korea", title: "Vice Chairman of the Commission on the Limits of the Continental Shelf" },
        { key: 22, boss: 12, name: "Abu Bakar Jaafar", nation: "Malaysia", title: "Chairman of the Editorial Committee", headOf: "Editorial Committee" },
        { key: 23, boss: 12, name: "Galo Carrera Hurtado", nation: "Mexico", title: "Chairman of the Training Committee", headOf: "Training Committee" },
        { key: 24, boss: 12, name: "Indurlall Fagoonee", nation: "Mauritius", title: "Member of the Commission on the Limits of the Continental Shelf" },
        { key: 25, boss: 12, name: "George Jaoshvili", nation: "Georgia", title: "Member of the Commission on the Limits of the Continental Shelf" },
        { key: 26, boss: 12, name: "Wenzhang Lu", nation: "China", title: "Member of the Commission on the Limits of the Continental Shelf" },
        { key: 27, boss: 12, name: "Isaac Owusu Orudo", nation: "Ghana", title: "Member of the Commission on the Limits of the Continental Shelf" },
        { key: 28, boss: 12, name: "Fernando Manuel Maia Pimentel", nation: "Portugal", title: "Member of the Commission on the Limits of the Continental Shelf" },
        { key: 7, boss: 1, name: "Renaud Sorieul", nation: "France", title: "International Trade Law Division Director", headOf: "International Trade Law Division" },
        { key: 13, boss: 7, name: "Other Employees" },
        { key: 8, boss: 1, name: "Annebeth Rosenboom", nation: "Netherlands", title: "Treaty Section Chief", headOf: "Treaty Section" },
        { key: 14, boss: 8, name: "Bradford Smith", nation: "United States", title: "Substantive Legal Issues Head", headOf: "Substantive Legal Issues" },
        { key: 29, boss: 14, name: "Other Employees" },
        { key: 15, boss: 8, name: "Andrei Kolomoets", nation: "Russia", title: "Technical/Legal Issues Head", headOf: "Technical/Legal Issues" },
        { key: 30, boss: 15, name: "Other Employees" },
        { key: 16, boss: 8, name: "Other Employees" },
        { key: 2, boss: 0, name: "Heads of Other Offices/Departments" }
    ];

    // create the Model with data for the tree, and assign to the Diagram
    myDiagram.model =
        $(go.TreeModel,
            {
                nodeParentKeyProperty: "parent",  // this property refers to the parent node data
                nodeDataArray: JSON.parse(JsonData)
            });

    // Overview
    myOverview =
        $(go.Overview, "myOverviewDiv",  // the HTML DIV element for the Overview
            { observed: myDiagram, contentAlignment: go.Spot.Center });   // tell it which Diagram to show and pan
}

// the Search functionality highlights all of the nodes that have at least one data property match a RegExp
function searchDiagram() {  // called by button
    var input = document.getElementById("mySearch");
    if (!input) return;
    input.focus();

    myDiagram.startTransaction("highlight search");

    if (input.value) {
        // search four different data properties for the string, any of which may match for success
        // create a case insensitive RegExp from what the user typed
        var regex = new RegExp(input.value, "i");
        var results = myDiagram.findNodesByExample({ FULL_NAME: regex },
            { POSITION_NUMBER: regex },
            { TITLE: regex },
            { GRADE: regex });
        myDiagram.highlightCollection(results);
        // try to center the diagram at the first node that was found
        if (results.count > 0) myDiagram.centerRect(results.first().actualBounds);
    } else {  // empty string only clears highlighteds collection
        myDiagram.clearHighlighteds();
    }

    myDiagram.commitTransaction("highlight search");
}

// Show the diagram's model in JSON format
function loadJSON(strJson) {
    if (strJson) {
        document.getElementById("mySavedModel").value = strJson;
    }
    else document.getElementById("mySavedModel").value = "{ \"class\": \"go.TreeModel\",  \"nodeDataArray\":\"\" }";

    myDiagram.model = go.Model.fromJson(document.getElementById("mySavedModel").value);
}

function GenerateImages(width, height) {

    console.log(width);
    console.log(height);

    // sanitize input
    width = parseInt(width);
    height = parseInt(height);
    if (isNaN(width)) width = 100;
    if (isNaN(height)) height = 100;

    // Give a minimum size of 50x50
    width = Math.max(width, 50);
    height = Math.max(height, 50);

    var imgDiv = document.getElementById('myImages');
    console.log(imgDiv)
    imgDiv.innerHTML = ""; // clear out the old images, if any

    var db = myDiagram.documentBounds.copy();
    //var boundswidth = db.width;
    //var boundsheight = db.height;
    //alert(boundswidth + ":" + boundsheight);
    var boundswidth = 10000;
    var boundsheight = 10000;
    var imgWidth = width;
    var imgHeight = height;
    var p = db.position.copy();



    //making images
    var img;
    img=myDiagram.makeImage({
        scale: 1,
        maxSize : new go.Size(11000, 8000)
    });

    //var doc = new jsPDF('l', 'pt', 'a0');
    //var idy = 0;
    //for (var idx = 0; idx < boundsheight; idx += imgHeight) {
    //    img = myDiagram.makeImage({
    //        scale: 1,
    //        type: "image/jpeg",
    //        background: "white",
    //        position: new go.Point(p.x + idy, p.y + idx),
    //        size: new go.Size(imgWidth, imgHeight)
    //    });
        
    //    if (idx==0)
    //        doc.addImage(img.src, 'JPEG', 0, 0, imgWidth, imgHeight);
    //    else
    //        doc.addImage(img.src, 'JPEG', idx-imgWidth, idy-imgHeight, imgWidth, imgHeight);
    //    idy += imgWidth
    //    document.getElementById("myImages").appendChild(img);
    //}
    

    
    var doc = new jsPDF('l', 'pt', 'a3');
    doc.addImage(img.src, 'JPEG', 0, 0, 1000, 800);
    //if you need more page use addPage();
    // doc.addPage();
    doc.save("diagram.pdf");
}

function load() {
    myDiagram.model = go.Model.fromJson(document.getElementById("mySavedModel").value);
}