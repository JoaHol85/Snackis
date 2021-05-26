﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ShowItem(buttonID, divID, buttonTextDivShowing, buttonTextDivHidden)
{
    const button = document.getElementById(buttonID);
    const div = document.getElementById(divID);

    if (div.classList.contains('d-none')) {
        div.classList.remove('d-none');
        button.textContent = buttonTextDivShowing;
    }
    else
    {
        div.classList.add('d-none');
        button.textContent = buttonTextDivHidden;
    }
};