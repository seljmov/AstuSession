// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    let PlaceHolderElement = $('#PlaceHolderHere')
    $('button[data-target="#AddUser"]').click(function (event) {
        let url = $(this).data('url');
        let decodeUrl = decodeURIComponent(url); 
        $.get(decodeUrl).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });

    PlaceHolderElement.on('click', '#SaveUser', function (event) {
        let form = $(this).parents('.modal').find('form');
        let actionUrl = form.attr('action');
        let sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            PlaceHolderElement.find('.modal').modal('hide');
            location.reload();
        });
    });
});

$(function () {
    let PlaceHolderElement = $('#PlaceHolderHere')
    $('button[data-target="#CreateSheet"]').click(function (event) {
        let url = $(this).data('url');
        let decodeUrl = decodeURIComponent(url);
        $.get(decodeUrl).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });

    PlaceHolderElement.on('click', '#SaveSheet', function (event) {
        let form = $(this).parents('.modal').find('form');
        let actionUrl = form.attr('action');
        let sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            PlaceHolderElement.find('.modal').modal('hide');
            location.reload();
        });
    });
});