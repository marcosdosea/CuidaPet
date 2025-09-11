// Navegação mobile e funcionalidades do design system

class MobileNavigation {
    constructor() {
        this.sidebar = document.getElementById('sidebar');
        this.mainWrapper = document.getElementById('mainWrapper');
        this.sidebarOverlay = document.getElementById('sidebarOverlay');
        this.isMobile = window.innerWidth <= 768;

        this.init();
    }

    init() {
        this.setupEventListeners();
        this.setupResponsiveHandling();
        this.setupInitialState();
    }

    setupEventListeners() {
        // Mobile menu button
        const mobileMenuBtn = document.querySelector('.mobile-menu-btn');
        if (mobileMenuBtn) {
            mobileMenuBtn.addEventListener('click', () => this.toggleSidebar());
        }

        // Sidebar close button
        const closeBtn = document.getElementById('closeMenu');
        if (closeBtn) {
            closeBtn.addEventListener('click', () => this.toggleSidebarMini());
        }

        // Logo arrow button
        const logoArrowBtn = document.getElementById('logoArrowBtn');
        if (logoArrowBtn) {
            logoArrowBtn.addEventListener('click', () => this.openSidebar());
        }

        // Overlay click
        if (this.sidebarOverlay) {
            this.sidebarOverlay.addEventListener('click', () => this.closeSidebar());
        }

        // Window resize
        window.addEventListener('resize', () => this.handleResize());

        // ESC key to close sidebar
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.isMobile) {
                this.closeSidebar();
            }
        });
    }

    setupResponsiveHandling() {
        this.handleResize();
    }

    setupInitialState() {
        if (this.isMobile) {
            this.closeSidebar();
        } else {
            // Para desktop, restaura o estado salvo
            const savedState = localStorage.getItem('sidebarState') || 'open';
            if (savedState === 'mini') {
                this.setSidebarMini();
            } else {
                this.openSidebar();
            }
        }
    }

    toggleSidebar() {
        if (this.isMobile) {
            // No mobile, apenas abre/fecha
            if (this.sidebar.classList.contains('active')) {
                this.closeSidebar();
            } else {
                this.openSidebar();
            }
        } else {
            // No desktop, alterna entre open e mini
            if (this.sidebar.classList.contains('mini')) {
                this.openSidebar();
            } else {
                this.setSidebarMini();
            }
        }
    }

    toggleSidebarMini() {
        if (this.isMobile) return;

        if (this.sidebar.classList.contains('mini')) {
            this.openSidebar();
        } else {
            this.setSidebarMini();
        }
    }

    openSidebar() {
        if (!this.sidebar || !this.mainWrapper) return;

        if (this.isMobile) {
            this.sidebar.classList.add('active');
            this.sidebar.classList.remove('mini');
            this.sidebarOverlay?.classList.add('active');
            this.mainWrapper.classList.remove('sidebar-open', 'sidebar-mini');

            // Prevent body scroll when sidebar is open on mobile
            document.body.style.overflow = 'hidden';
        } else {
            this.sidebar.classList.add('active');
            this.sidebar.classList.remove('mini');
            this.mainWrapper.classList.add('sidebar-open');
            this.mainWrapper.classList.remove('sidebar-mini');
            this.sidebarOverlay?.classList.remove('active');

            localStorage.setItem('sidebarState', 'open');
        }
    }

    setSidebarMini() {
        if (!this.sidebar || !this.mainWrapper || this.isMobile) return;

        this.sidebar.classList.add('mini');
        this.sidebar.classList.remove('active');
        this.mainWrapper.classList.add('sidebar-mini');
        this.mainWrapper.classList.remove('sidebar-open');
        this.sidebarOverlay?.classList.remove('active');

        localStorage.setItem('sidebarState', 'mini');
    }

    closeSidebar() {
        if (!this.sidebar || !this.mainWrapper) return;

        this.sidebar.classList.remove('active', 'mini');
        this.mainWrapper.classList.remove('sidebar-open', 'sidebar-mini');
        this.sidebarOverlay?.classList.remove('active');

        // Restore body scroll
        document.body.style.overflow = '';
    }

    handleResize() {
        const wasMobile = this.isMobile;
        this.isMobile = window.innerWidth <= 768;

        if (wasMobile !== this.isMobile) {
            // Reset styles when switching between mobile/desktop
            if (this.isMobile) {
                this.closeSidebar();
            } else {
                // Restore desktop state
                const savedState = localStorage.getItem('sidebarState') || 'open';
                if (savedState === 'mini') {
                    this.setSidebarMini();
                } else {
                    this.openSidebar();
                }
            }
        }
    }
}

// Search functionality
class SearchManager {
    constructor() {
        this.searchInputs = document.querySelectorAll('.search-input');
        this.init();
    }

    init() {
        this.searchInputs.forEach(input => {
            const clearBtn = input.parentElement.querySelector('.search-clear-btn');

            // Show/hide clear button
            input.addEventListener('input', () => {
                if (clearBtn) {
                    clearBtn.style.display = input.value ? 'block' : 'none';
                }
            });

            // Clear functionality
            if (clearBtn) {
                clearBtn.addEventListener('click', () => {
                    input.value = '';
                    clearBtn.style.display = 'none';
                    input.focus();

                    // Trigger input event to update any filtering
                    input.dispatchEvent(new Event('input', { bubbles: true }));
                });
            }
        });
    }
}

// Quantity controls
class QuantityManager {
    constructor() {
        this.quantityControls = document.querySelectorAll('.product-actions');
        this.init();
    }

    init() {
        this.quantityControls.forEach(control => {
            const plusBtn = control.querySelector('.btn-quantity.plus');
            const minusBtn = control.querySelector('.btn-quantity.minus');
            const quantitySpan = control.querySelector('.quantity');

            if (plusBtn && minusBtn && quantitySpan) {
                plusBtn.addEventListener('click', (e) => {
                    e.stopPropagation();
                    this.incrementQuantity(quantitySpan);
                    this.addFeedbackAnimation(plusBtn);
                });

                minusBtn.addEventListener('click', (e) => {
                    e.stopPropagation();
                    this.decrementQuantity(quantitySpan);
                    this.addFeedbackAnimation(minusBtn);
                });
            }
        });
    }

    incrementQuantity(quantitySpan) {
        let quantity = parseInt(quantitySpan.textContent) || 0;
        quantity++;
        quantitySpan.textContent = quantity;
        this.updateQuantityVisibility(quantitySpan.parentElement, quantity);
    }

    decrementQuantity(quantitySpan) {
        let quantity = parseInt(quantitySpan.textContent) || 0;
        if (quantity > 0) {
            quantity--;
            quantitySpan.textContent = quantity;
            this.updateQuantityVisibility(quantitySpan.parentElement, quantity);
        }
    }

    updateQuantityVisibility(control, quantity) {
        const minusBtn = control.querySelector('.btn-quantity.minus');
        if (minusBtn) {
            minusBtn.style.opacity = quantity > 0 ? '1' : '0.5';
        }
    }

    addFeedbackAnimation(button) {
        button.style.transform = 'scale(0.9)';
        setTimeout(() => {
            button.style.transform = 'scale(1)';
        }, 150);
    }
}

// Filter toggle functionality
class FilterManager {
    constructor() {
        this.filterBtns = document.querySelectorAll('.filter-btn');
        this.init();
    }

    init() {
        this.filterBtns.forEach(btn => {
            btn.addEventListener('click', (e) => {
                const filter = btn.dataset.filter;
                this.setActiveFilter(btn);
                this.handleFilterChange(filter);
            });
        });
    }

    setActiveFilter(activeBtn) {
        this.filterBtns.forEach(btn => btn.classList.remove('active'));
        activeBtn.classList.add('active');
    }

    handleFilterChange(filter) {
        // Implement navigation or filtering logic based on filter type
        if (filter === 'petshops') {
            // Navigate to petshops view
            if (window.location.pathname.includes('ConsultarItens')) {
                window.location.href = '/ConsultarEstabelecimentos';
            }
        } else if (filter === 'itens') {
            // Navigate to items view
            if (window.location.pathname.includes('ConsultarEstabelecimentos')) {
                window.location.href = '/ConsultarItens';
            }
        }
    }
}

// Notification system
class NotificationManager {
    constructor() {
        this.notificationBtn = document.getElementById('notificationBtn');
        this.notificationModal = document.getElementById('notificationModal');
        this.init();
    }

    init() {
        if (this.notificationBtn) {
            this.notificationBtn.addEventListener('click', () => {
                this.openNotificationModal();
            });
        }
    }

    openNotificationModal() {
        if (this.notificationModal) {
            const modal = new bootstrap.Modal(this.notificationModal);
            modal.show();
            this.loadNotifications();
        }
    }

    loadNotifications() {
        // Implement notification loading logic
        console.log('Loading notifications...');
    }
}

// Table responsiveness helper
class TableManager {
    constructor() {
        this.tables = document.querySelectorAll('.table-responsive');
        this.init();
    }

    init() {
        // Garante que as tabelas não causem overflow horizontal
        this.tables.forEach(table => {
            table.style.width = '100%';
            table.style.maxWidth = '100%';
        });

        // Monitora mudanças de tamanho
        window.addEventListener('resize', () => this.handleResize());
    }

    handleResize() {
        this.tables.forEach(table => {
            // Force recalculation of table layout
            const originalDisplay = table.style.display;
            table.style.display = 'none';
            table.offsetHeight; // Trigger reflow
            table.style.display = originalDisplay;
        });
    }
}

// Initialize everything when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    // Initialize components
    const navigation = new MobileNavigation();
    const searchManager = new SearchManager();
    const quantityManager = new QuantityManager();
    const filterManager = new FilterManager();
    const notificationManager = new NotificationManager();
    const tableManager = new TableManager();

    // Make navigation available globally for backward compatibility
    window.toggleSidebar = () => navigation.toggleSidebar();
    window.closeSidebar = () => navigation.closeSidebar();

    // Smooth scroll for better UX
    document.documentElement.style.scrollBehavior = 'smooth';

    // Add loading state management
    window.addEventListener('beforeunload', () => {
        document.body.classList.add('loading');
    });

    // Prevent horizontal scroll on body
    document.body.style.overflowX = 'hidden';
});

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        MobileNavigation,
        SearchManager,
        QuantityManager,
        FilterManager,
        NotificationManager,
        TableManager
    };
}