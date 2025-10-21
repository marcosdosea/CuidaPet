$(() => {
    const sidebar = $('#sidebar');
    const mainWrapper = $('#mainWrapper');
    const btnMenu = $('#btnMenu');
    const closeMenu = $('#closeMenu');
    const menuToggle = $('#menuToggle');
    const logoArrowBtn = $('#logoArrowBtn');

    const sidebarStates = {
        open: {
            sidebarClasses: ['active'],
            wrapperClasses: ['sidebar-open'],
            removeClasses: ['mini', 'sidebar-mini']
        },
        mini: {
            sidebarClasses: ['mini'],
            wrapperClasses: ['sidebar-mini'],
            removeClasses: ['active', 'sidebar-open']
        },
        closed: {
            sidebarClasses: [],
            wrapperClasses: [],
            removeClasses: ['active', 'mini', 'sidebar-open', 'sidebar-mini']
        }
    };

    function applySidebarState(state, withTransition = false) {
        const config = sidebarStates[state];

        if (withTransition) {
            sidebar.addClass('transitioning');
            mainWrapper.addClass('transitioning');
        }

        sidebar.removeClass(config.removeClasses.join(' '));
        mainWrapper.removeClass(config.removeClasses.join(' '));

        if (config.sidebarClasses.length) {
            sidebar.addClass(config.sidebarClasses.join(' '));
        }
        if (config.wrapperClasses.length) {
            mainWrapper.addClass(config.wrapperClasses.join(' '));
        }

        if (state !== 'closed') {
            localStorage.setItem('sidebarState', state);
        }

        if (withTransition) {
            setTimeout(() => {
                sidebar.removeClass('transitioning');
                mainWrapper.removeClass('transitioning');
            }, 300);
        }
    }

    const savedState = localStorage.getItem('sidebarState') || 'open';
    applySidebarState(savedState);

    btnMenu.on('click', () => {
        applySidebarState('open');
        menuToggle.removeClass('show');
    });

    closeMenu.on('click', () => {
        applySidebarState('mini', true);
        menuToggle.addClass('show');
    });

    logoArrowBtn.on('click', () => {
        applySidebarState('open', true);
    });

    menuToggle.on('click', function () {
        applySidebarState('open');
        $(this).removeClass('show');
    });

    function setActiveLink(activeElement) {
        $('.sidebar a').removeClass('active-link');
        $(activeElement).addClass('active-link');
    }

    $('.sidebar a').on('click', function () {
        setActiveLink(this);
    });

    const currentPath = window.location.pathname.toLowerCase();
    $('.sidebar a').each(function () {
        const linkHref = $(this).attr('href');
        if (linkHref) {
            const linkPath = linkHref.toLowerCase();
            if (currentPath.includes(linkPath) && linkPath !== '/' && linkPath.length > 1) {
                setActiveLink(this);
                return false;
            }
        }
    });

    function checkResponsive() {
        const isDesktop = $(window).width() > 768;
        const currentState = isDesktop ? savedState : 'closed';
        applySidebarState(currentState);
        menuToggle.toggleClass('show', !isDesktop);
    }

    $(window).on('resize', checkResponsive);
    checkResponsive();

    // ===== FUNCIONALIDADE DE BADGE DE NOTIFICAÇÕES =====
    const notificationBadge = $('#notificationBadge');

    window.updateNotificationBadge = function() {
        $.ajax({
            url: '/Notificacao/GetContagemNaoLidas',
            type: 'GET',
            dataType: 'json'
        })
        .done(function (response) {
            if (response && response.success) {
                const count = response.count || 0;
                if (count > 0) {
                    notificationBadge.text(count > 99 ? '99+' : count).show();
                } else {
                    notificationBadge.hide();
                }
            }
        })
        .fail(function (xhr, status, error) {
            console.error('Erro ao atualizar contagem de notificações:', error);
        });
    };

    window.updateNotificationBadge();

    setInterval(function () {
        window.updateNotificationBadge();
    }, 2 * 60 * 1000);
});