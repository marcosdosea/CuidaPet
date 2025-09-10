$(document).ready(function () {
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

    // ===== FUNCIONALIDADE DE NOTIFICAÇÕES =====
    const TESTE_ID_PESSOA = 1;

    let notificationsCache = [];
    let lastUpdateTime = 0;
    const CACHE_DURATION = 5 * 60 * 1000; // 5 minutos

    const notificationBtn = $('#notificationBtn');
    const notificationModalEl = document.getElementById('notificationModal');
    const notificationModal = notificationModalEl ? new bootstrap.Modal(notificationModalEl) : null;

    const notificationBadge = $('#notificationBadge');
    const notificationLoader = $('#notificationLoader');
    const notificationContent = $('#notificationContent');
    const notificationEmpty = $('#notificationEmpty');
    const notificationList = $('#notificationList');

    function formatDate(dateString) {
        const date = new Date(dateString);
        const now = new Date();
        const diff = now - date;

        const minutes = Math.floor(diff / 60000);
        const hours = Math.floor(diff / 3600000);
        const days = Math.floor(diff / 86400000);

        if (minutes < 1) return 'Agora mesmo';
        if (minutes < 60) return `${minutes}m atrás`;
        if (hours < 24) return `${hours}h atrás`;
        if (days < 7) return `${days}d atrás`;

        return date.toLocaleDateString('pt-BR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    }

    function getNotificationIcon(titulo) {
        const titulo_lower = (titulo || '').toLowerCase();
        if (titulo_lower.includes('vacina')) return 'bi-shield-check';
        if (titulo_lower.includes('consulta')) return 'bi-calendar-check';
        if (titulo_lower.includes('medicamento')) return 'bi-capsule';
        if (titulo_lower.includes('pet')) return 'bi-heart';
        if (titulo_lower.includes('lembrete')) return 'bi-bell';
        return 'bi-info-circle';
    }

    function renderNotifications(notifications) {
        if (!notifications || notifications.length === 0) {
            notificationContent.hide();
            notificationEmpty.show();
            updateNotificationBadge(0);
            return;
        }

        notificationEmpty.hide();
        notificationContent.show();

        const notificationHtml = notifications.map((notif, index) => {
            const icon = getNotificationIcon(notif.titulo);
            const isUnread = typeof notif.statusLida === 'number' ? notif.statusLida === 0 : true;

            return `
                <div class="notification-item ${isUnread ? 'unread' : ''}" 
                     data-notification-id="${notif.id}"
                     style="animation-delay: ${index * 0.1}s">
                    <div class="notification-item-flex">
                        <div class="notification-icon">
                            <i class="bi ${icon}"></i>
                        </div>
                        <div class="notification-content">
                            <div class="notification-title">${notif.titulo ?? ''}</div>
                            ${notif.descricao ? `<div class="notification-description">${notif.descricao}</div>` : ''}
                            <div class="notification-date">${formatDate(notif.dataEnvio)}</div>
                        </div>
                    </div>
                </div>
            `;
        }).join('');

        notificationList.html(notificationHtml);

        const unreadCount = notifications.filter(n => typeof n.statusLida === 'number' ? n.statusLida === 0 : true).length;
        updateNotificationBadge(unreadCount);
    }

    function updateNotificationBadge(count) {
        if (count > 0) {
            notificationBadge.text(count > 99 ? '99+' : count).show();
        } else {
            notificationBadge.hide();
        }
    }

    // Sempre retorna Promise nativa e use .finally no chamador
    function loadNotifications(forceRefresh = false) {
        const now = Date.now();

        if (!forceRefresh && notificationsCache.length > 0 && (now - lastUpdateTime) < CACHE_DURATION) {
            renderNotifications(notificationsCache);
            return Promise.resolve();
        }

        return new Promise((resolve) => {
            $.ajax({
                url: '/Notificacao/GetNotificacoes',
                type: 'GET',
                data: { idPessoa: TESTE_ID_PESSOA },
                dataType: 'json'
            })
                .done(function (response) {
                    if (response && response.success) {
                        notificationsCache = response.data || [];
                        lastUpdateTime = now;
                        renderNotifications(notificationsCache);
                    } else {
                        console.error('Erro ao carregar notificações:', response ? response.message : 'resposta inválida');
                        notificationContent.hide();
                        notificationEmpty.show();
                        updateNotificationBadge(0);
                    }
                })
                .fail(function (xhr, status, error) {
                    console.error('Erro na requisição de notificações:', error);
                    notificationContent.hide();
                    notificationEmpty.show();
                    updateNotificationBadge(0);
                })
                .always(function () {
                    resolve();
                });
        });
    }

    function markNotificationAsRead(notificationId) {
        return $.ajax({
            url: '/Notificacao/MarcarComoLidaAjax',
            type: 'POST',
            data: {
                idNotificacao: notificationId,
                idPessoa: TESTE_ID_PESSOA
            }
        });
    }

    // Abrir modal (Bootstrap 5)
    notificationBtn.on('click', function () {
        if (!notificationModal) return;
        // mostra loader imediatamente
        notificationLoader.show();
        notificationContent.hide();
        notificationEmpty.hide();

        notificationModal.show();
    });

    // Quando o modal estiver visível, carrega as notificações
    if (notificationModalEl) {
        notificationModalEl.addEventListener('shown.bs.modal', function () {
            loadNotifications().finally(function () {
                notificationLoader.hide();
            });
        });
    }

    // Clique em notificação
    $(document).on('click', '.notification-item', function () {
        const $notificationItem = $(this);
        const notificationId = $notificationItem.data('notification-id');
        const isUnread = $notificationItem.hasClass('unread');

        if (isUnread) {
            markNotificationAsRead(notificationId)
                .done(function (response) {
                    if (response && response.success) {
                        $notificationItem.removeClass('unread');
                        const cached = notificationsCache.find(n => n.id === notificationId);
                        if (cached) cached.statusLida = 1;
                        const unreadCount = $('.notification-item.unread').length;
                        updateNotificationBadge(unreadCount);
                    }
                })
                .fail(function () {
                    console.error('Erro ao marcar notificação como lida');
                });
        }

        console.log('Notificação clicada:', notificationId);
    });

    // Inicial: atualizar apenas o badge
    loadNotifications().finally(function () {
        // nada a fazer aqui
    });

    // Atualização periódica
    setInterval(function () {
        loadNotifications(true);
    }, 2 * 60 * 1000);
});