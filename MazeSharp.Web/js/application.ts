var maze;
var stop;
var visited;
var totalMoves;
var totalCells;

function clearMaze() {
    /* get the HTML element for the container to hold the maze */
    /* and delete the old one if any */
    var box = document.getElementById("box");
    var oldMaze = document.getElementById("maze");
    if (oldMaze != null) {
        box.removeChild(oldMaze);
    }
}

function setBoxSize(mazeWidth: number, cellSize: number) {
    /* set the width of the box based on the size of the maze and the size of the cells */
    var box = document.getElementById("box");
    box.style.width = ((mazeWidth * cellSize) + 2) + "px";
}

function build(newMaze) {
    var resetCounter = m => {
        $(".visited-number").show();
        $("#visited-number").html("0");
        $('#visited-counter').circleProgress({
            value: 0,
            size: 50,
            animation: false,
            fill: {
                gradient: ["green", "lightgreen"]
            }
        });
    };
    maze = newMaze;

    var above = 0;
    var below = 2;
    var left = 3;
    var right = 1;

    /**
     * Create an HTML element that represents an individual cell
     */
    function createCell(x: number, y: number) {
        var cell = maze.Cells[x][y];
        var wall = cell.Wall;
        var div = document.createElement("div");
        var classes = "cell";
        if (x === 0) {
            classes = classes + (wall[left] ? " left" : " noleft");
        }

        if (y === 0) {
            classes = classes + (wall[above] ? " top" : " notop");
        }

        if (x < (maze.Width - 1)) {
            classes = classes + (wall[right] ? " right" : " noright");
        }
        else {
            classes = classes + (wall[right] ? " right" : "");
        }

        if (y < (maze.Height - 1)) {
            classes = classes + (wall[below] ? " bottom" : " nobottom");
        }
        else {
            classes = classes + (wall[below] ? " bottom" : "");
        }

        if (cell.IsStart) {
            classes += " player";
        }

        if (cell.IsEnd) {
            classes += " end";
        }

        div.setAttribute("class", classes);
        div.setAttribute("data-x", x.toString());
        div.setAttribute("data-y", y.toString());

        cell.divElement = div;

        return cell;
    }

    clearMaze();
    setBoxSize(maze.Width, 10);
    totalCells = maze.Width * maze.Height;
    $("#total-cells").html(totalCells);
    resetCounter(maze);
    
    /* create the element to hold the maze itself */
    var mazeRoot = document.createElement("div");
    mazeRoot.setAttribute("id", "maze");

    for (var y = 0; y < maze.Height; y++) {
        var row = document.createElement("div");
        row.setAttribute("class", "mrow");
        mazeRoot.appendChild(row);

        for (var x = 0; x < maze.Width; x++) {
            var cell = createCell(x, y);
            cell.visited = false;

            row.appendChild(cell.divElement);
        }
    }

    var box = document.getElementById("box");
    box.appendChild(mazeRoot);
}

function getNextMove() {
    $.ajax({
        data: {
            currentX: maze.CurrentPosition.X,
            currentY: maze.CurrentPosition.Y
        },
        url: "/home/move",
        success: cell => {
            move(cell);
        }
    });
}

function move(cell) {
    totalMoves++;

    console.log(totalMoves);
    var currentPosition = maze.Cells[maze.CurrentPosition.X][maze.CurrentPosition.Y];
    var divCurrent = currentPosition.divElement;
    $(divCurrent).removeClass("player");
    $(divCurrent).addClass("visited");

    var nextPosition = maze.Cells[cell.X][cell.Y];
    var divNext = nextPosition.divElement;
    // if next position has visited class already then add backtracked class instead of player class
    if ($(divNext).hasClass("visited")) {
        $(divNext).removeClass("visited");
        $(divNext).addClass("backtracked");
    } else {
        $(divNext).addClass("player");
        visited++;
    }
    var efficiency = visited / totalMoves;
    $("#visited-number").html(visited);
    $("#total-moves").html(totalMoves);
    $("#algorithm-efficiency").html(efficiency.toString());
    $("#visited-counter").circleProgress({
        value: visited / totalMoves
    });

    maze.CurrentPosition.X = nextPosition.X;
    maze.CurrentPosition.Y = nextPosition.Y;
    if (!((maze.CurrentPosition.X === maze.End.X && maze.CurrentPosition.Y === maze.End.Y) || stop === true)) {
        getNextMove();
    }
}

$(() => {

    $("#countdown").countdown360({
        radius: 60,
        seconds: 60,
        fontColor: "#FFFFFF",
        autostart: false,
        onComplete: function () { stop = true; }
    }).stop();

    $("#generate-maze").click(() => {
        var width = $("#maze-width").val();
        var height = $("#maze-height").val();
        var perfect = $("#is-perfect").val();
        $.ajax({
            data: {
                width: width,
                height: height,
                perfect: perfect
            },
            url: "/home/generate",
            success: maze => {
                build(maze);
            }
        });
    });

    $('#player').change(function () {
        console.log($(this).val());
        console.log(this.files[0].pathname);
        console.log(this.files[0].filename);
        console.log(this.files[0].mozFullPath);
    });

    $("#clear-maze").click(() => {
        clearMaze();
    });

    $("#load-player").click(() => {

    });

    $("#solve-maze").click(() => {
        stop = false;
        visited = 1;
        //totalCells = 1;
        totalMoves = 1;
        $("#countdown").countdown360().start();
        getNextMove();
    });

    $("#solve-stop").click(() => {
        $("#countdown").countdown360().stop();
        stop = true;
    });
});