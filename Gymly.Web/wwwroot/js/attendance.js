(function () {
    'use strict';

    const config = window.gymlyAttendance || {};
    const searchUrl = config.searchUrl;

    const qrInput = document.getElementById('qrTokenInput');
    const qrForm = document.getElementById('qrForm');
    const searchInput = document.getElementById('memberSearch');
    const resultsList = document.getElementById('searchResults');
    const selectedIdInput = document.getElementById('selectedMemberId');
    const selectedDisplay = document.getElementById('selectedMemberDisplay');
    const manualSubmit = document.getElementById('manualSubmit');
    const manualForm = document.getElementById('manualForm');

    if (qrInput && qrForm) {
        qrInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                qrForm.submit();
            }
        });

        qrInput.addEventListener('input', function () {
            var val = qrInput.value.trim();
            if (val.length >= 32) {
                qrForm.submit();
            }
        });
    }

    if (!searchInput || !resultsList || !selectedIdInput || !selectedDisplay || !manualSubmit) {
        return;
    }

    let debounceTimer = null;
    let currentRequest = null;

    function escapeHtml(value) {
        if (value === null || value === undefined) return '';
        return String(value)
            .replace(/&/g, '&')
            .replace(/</g, '<')
            .replace(/>/g, '>')
            .replace(/"/g, '"')
            .replace(/'/g, '&#39;');
    }

    function clearResults() {
        resultsList.innerHTML = '';
        resultsList.style.display = 'none';
    }

    function clearSelection() {
        selectedIdInput.value = '';
        selectedDisplay.innerHTML = '';
        selectedDisplay.style.display = 'none';
        manualSubmit.disabled = true;
    }

    function selectMember(member) {
        selectedIdInput.value = String(member.id);
        const status = member.isActive
            ? '<span style="color: green;">active</span>'
            : '<span style="color: red;">inactive</span>';
        selectedDisplay.innerHTML =
            '<strong>' + escapeHtml(member.name) + '</strong> ' + status +
            ' &mdash; ' + escapeHtml(member.email) +
            (member.phone ? ' &mdash; ' + escapeHtml(member.phone) : '') +
            ' &nbsp; <a href="#" id="clearSelectionLink" style="color: #c00;">clear</a>';
        selectedDisplay.style.display = 'block';
        manualSubmit.disabled = false;
        clearResults();
        searchInput.value = '';

        const clearLink = document.getElementById('clearSelectionLink');
        if (clearLink) {
            clearLink.addEventListener('click', function (e) {
                e.preventDefault();
                clearSelection();
                searchInput.focus();
            });
        }
    }

    function renderResults(members) {
        clearResults();
        if (!members || members.length === 0) {
            return;
        }
        const html = members.map(function (m) {
            const status = m.isActive ? 'active' : 'inactive';
            const statusColor = m.isActive ? 'green' : 'red';
            return (
                '<li data-id="' + m.id + '" style="padding: 6px; border-bottom: 1px solid #eee; cursor: pointer;">' +
                    '<div><strong>' + escapeHtml(m.name) + '</strong> ' +
                        '<span style="color: ' + statusColor + '; font-size: 0.85em;">(' + status + ')</span>' +
                    '</div>' +
                    '<div style="color: #666; font-size: 0.85em;">' +
                        escapeHtml(m.email) +
                        (m.phone ? ' &middot; ' + escapeHtml(m.phone) : '') +
                    '</div>' +
                '</li>'
            );
        }).join('');
        resultsList.innerHTML = html;
        resultsList.style.display = 'block';

        Array.prototype.forEach.call(resultsList.querySelectorAll('li'), function (li) {
            li.addEventListener('click', function () {
                const id = parseInt(li.getAttribute('data-id'), 10);
                const member = members.find(function (m) { return m.id === id; });
                if (member) {
                    selectMember(member);
                }
            });
        });
    }

    function fetchResults(term) {
        if (currentRequest && currentRequest.abort) {
            currentRequest.abort();
        }
        if (typeof AbortController === 'undefined') {
            fetchResultsLegacy(term);
            return;
        }
        const controller = new AbortController();
        currentRequest = controller;
        const url = searchUrl + '?term=' + encodeURIComponent(term);
        fetch(url, { signal: controller.signal, headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (r) { return r.json(); })
            .then(function (data) { renderResults(data); })
            .catch(function (err) {
                if (err && err.name === 'AbortError') return;
                clearResults();
            });
    }

    function fetchResultsLegacy(term) {
        const url = searchUrl + '?term=' + encodeURIComponent(term);
        fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (r) { return r.json(); })
            .then(function (data) { renderResults(data); })
            .catch(function () { clearResults(); });
    }

    searchInput.addEventListener('input', function () {
        const term = searchInput.value.trim();
        if (term.length < 2) {
            clearResults();
            return;
        }
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(function () { fetchResults(term); }, 250);
    });

    searchInput.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            clearResults();
        }
    });

    document.addEventListener('click', function (e) {
        if (!manualForm.contains(e.target)) {
            clearResults();
        }
    });
})();
