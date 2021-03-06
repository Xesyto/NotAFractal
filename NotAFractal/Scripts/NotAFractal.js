﻿$(document).ready(function() {
    $(".toggleLink").click(toggle);
    $(".infoLink").click(typeInfo);
});

//Generates the content of a node
generate = function(nodeType, nodeSeed, element) {
    $.get('/Node/GetNode/' + nodeType + '/' + nodeSeed, function(data) {
        //Add new nodes to page then mark this node as having generated its children
        $(element).append(data);
        $(element).data('generated', true);

        //Associate click events to new nodes, so they in turn can be expanded or have their information displayed
        $(element).children(".nodeContainer").find('.toggleLink').click(toggle);
        $(element).children(".nodeContainer").find('.infoLink').click(typeInfo);

        //Finally open up the node for view
        $(element).data('open', true);        
        $(element).children(".nodeContainer").toggle('fast');
        $(element).children(".toggleLink").children(".treeViewToggleIndicator").text("-");
    });
};

toggle = function() {
    {
        var toggleIndicator = $(this).find(".treeViewToggleIndicator");
        var node = $(this).parent();

        //If this node has not been generated yet ( if its just a title and no content) that is, if its never been opened before
        //Generate its .nodeContainer through an ajax call

        if (node.data('generated') == false) {
            generate(node.data('type'), node.data('seed'), node);
        } else {
            //Togle the visible part of the node
            node.children(".nodeContainer").toggle('fast');

            //make the + a - or reverse
            if (node.data('open')) {
                toggleIndicator.text("+");
                node.data('open', false);
            } else {
                toggleIndicator.text("-");
                node.data('open', true);
            }
        }
    }
    ;
};

//source: http://css-tricks.com/scrollfollow-sidebar/
$(function () {

    var $sidebar = $("#sidebar"),
        $window = $(window),
        offset = $sidebar.offset(),
        topPadding = 15;

    $window.scroll(function () {
        if ($window.scrollTop() > offset.top) {
            $sidebar.stop().animate({
                marginTop: $window.scrollTop() - offset.top + topPadding
            });
        } else {
            $sidebar.stop().animate({
                marginTop: 0
            });
        }
    });

});

typeInfo = function() {
    var node = $(this).parent();

    getInfo(node.data('type'), node.data('seed'));
};

getInfo = function(nodeType, nodeSeed) {
    $.get('/Node/GetNodeInformation/'+nodeType+'/'+nodeSeed, function (data) {
        $('.mainSidebar').html(data);
    });
};