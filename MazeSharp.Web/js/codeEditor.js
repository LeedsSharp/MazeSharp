var CodeMirror; // /libs/CodeMirror
var riot; // /libs/riot.js
var _; // /libs/lodash.min.js
$(function () {
    var getCodeObjFromCode = function (c) { throw new Error("Where is this, find in ElevatorSharp"); };
    var createEditor = function () {
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
                Tab: function (cm) {
                    var spaces = new Array(cm.getOption("indentUnit") + 1).join(" ");
                    cm.replaceSelection(spaces);
                }
            }
        });
        // reindent on paste (adapted from https://github.com/ahuth/brackets-paste-and-indent/blob/master/main.js)
        cm.on("change", function (codeMirror, change) {
            if (change.origin !== "paste") {
                return;
            }
            function reindentLines(lineFrom, lineTo) {
                codeMirror.operation(function () {
                    codeMirror.eachLine(lineFrom, lineTo, function (lineHandle) {
                        codeMirror.indentLine(lineHandle.lineNo(), "smart");
                    });
                });
            }
            reindentLines(change.from.line, change.from.line + change.text.length);
        });
        var reset = function () {
            cm.setValue($("#mvc-elevator-implementation").text().trim());
        };
        var saveCode = function () {
            // Don't save in mvc mode
            //localStorage.setItem(lsKey, cm.getValue());
            //$("#save_message").text("Code saved " + new Date().toTimeString());
            //returnObj.trigger("change");
        };
        //var existingCode = localStorage.getItem(lsKey);
        var existingCode = false; // don't use local storage in mvc mode
        if (existingCode) {
            cm.setValue(existingCode);
        }
        else {
            reset();
        }
        $("#button_save").click(function () {
            saveCode();
            cm.focus();
        });
        $("#button_reset").click(function () {
            if (confirm("Do you really want to reset to the default implementation?")) {
                localStorage.setItem("develevateBackupCode", cm.getValue());
                reset();
            }
            cm.focus();
        });
        $("#button_resetundo").click(function () {
            if (confirm("Do you want to bring back the code as before the last reset?")) {
                cm.setValue(localStorage.getItem("develevateBackupCode") || "");
            }
            cm.focus();
        });
        var returnObj = riot.observable({});
        var autoSaver = _.debounce(saveCode, 1000);
        cm.on("change", function () {
            autoSaver();
        });
        returnObj.getCodeObj = function () {
            console.log("Getting code...");
            var code = cm.getValue();
            var obj;
            try {
                obj = getCodeObjFromCode(code);
                returnObj.trigger("code_success");
            }
            catch (e) {
                returnObj.trigger("usercode_error", e);
                return null;
            }
            return obj;
        };
        returnObj.setCode = function (code) {
            cm.setValue(code);
        };
        returnObj.getCode = function () { return cm.getValue(); };
        returnObj.setDevTestCode = function () {
            cm.setValue($("#devtest-elev-implementation").text().trim());
        };
        $("#button_apply").click(function () {
            returnObj.trigger("apply_code");
        });
        return returnObj;
    };
    var editor = createEditor();
});
