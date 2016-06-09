var CodeMirror:any; // /libs/CodeMirror
var riot: any; // /libs/riot.js
var _:any; // /libs/lodash.min.js

$(() => {
    var getCodeObjFromCode = (c) => { throw new Error("Where is this, find in ElevatorSharp"); };
    
    var createEditor = () => {
        var lsKey = "elevatorCrushCode_v5";

        var cm = CodeMirror.fromTextArea(document.getElementById("code"), {
            lineNumbers: true,
            indentUnit: 4,
            indentWithTabs: false,
            theme: "solarized light",
            mode: "javascript",
            autoCloseBrackets: true,
            extraKeys: {
                // the following Tab key mapping is from http://codemirror.net/doc/manual.html#keymaps
                Tab(cm) {
                    var spaces = new Array(cm.getOption("indentUnit") + 1).join(" ");
                    cm.replaceSelection(spaces);
                }
            }
        });

        // reindent on paste (adapted from https://github.com/ahuth/brackets-paste-and-indent/blob/master/main.js)
        cm.on("change", (codeMirror, change) => {
            if (change.origin !== "paste") {
                return;
            }

            function reindentLines(lineFrom, lineTo) {
                codeMirror.operation(() => {
                    codeMirror.eachLine(lineFrom, lineTo, lineHandle => {
                        codeMirror.indentLine(lineHandle.lineNo(), "smart");
                    });
                });
            }

            reindentLines(change.from.line, change.from.line + change.text.length);
        });

        var reset = () => {
            cm.setValue($("#mvc-elevator-implementation").text().trim());
        };


        var saveCode = () => {
            // Don't save in mvc mode

            //localStorage.setItem(lsKey, cm.getValue());
            //$("#save_message").text("Code saved " + new Date().toTimeString());
            //returnObj.trigger("change");
        };

        //var existingCode = localStorage.getItem(lsKey);
        var existingCode = false; // don't use local storage in mvc mode
        if (existingCode) {
            cm.setValue(existingCode);
        } else {
            reset();
        }

        $("#button_save").click(() => {
            saveCode();
            cm.focus();
        });

        $("#button_reset").click(() => {
            if (confirm("Do you really want to reset to the default implementation?")) {
                localStorage.setItem("develevateBackupCode", cm.getValue());
                reset();
            }
            cm.focus();
        });

        $("#button_resetundo").click(() => {
            if (confirm("Do you want to bring back the code as before the last reset?")) {
                cm.setValue(localStorage.getItem("develevateBackupCode") || "");
            }
            cm.focus();
        });

        var returnObj = riot.observable({});
        var autoSaver = _.debounce(saveCode, 1000);
        cm.on("change", () => {
            autoSaver();
        });

        returnObj.getCodeObj = () => {
            console.log("Getting code...");
            var code = cm.getValue();
            var obj;
            try {
                obj = getCodeObjFromCode(code);
                returnObj.trigger("code_success");
            } catch (e) {
                returnObj.trigger("usercode_error", e);
                return null;
            }
            return obj;
        };
        returnObj.setCode = code => {
            cm.setValue(code);
        };
        returnObj.getCode = () => cm.getValue();
        returnObj.setDevTestCode = () => {
            cm.setValue($("#devtest-elev-implementation").text().trim());
        }

        $("#button_apply").click(() => {
            returnObj.trigger("apply_code");
        });
        return returnObj;
    };

    var editor = createEditor();
});