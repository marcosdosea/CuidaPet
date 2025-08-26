let sortDirection = {};
let searchTimeout;

// Cache de elementos DOM
const elements = {
    table: null,
    tbody: null,
    searchInput: null,
    init() {
        this.table = document.getElementById('table');
        this.tbody = this.table.querySelector('tbody');
        this.searchInput = document.getElementById('searchInput');
    }
};

function sortTable(columnIndex, dataType) {
    const rows = Array.from(elements.tbody.querySelectorAll('tr'));

    // Determinar direção da ordenação
    const currentDirection = sortDirection[columnIndex] || 'asc';
    const newDirection = currentDirection === 'asc' ? 'desc' : 'asc';
    sortDirection[columnIndex] = newDirection;

    // Resetar todas as setas
    document.querySelectorAll('.sort-arrows i').forEach(icon => icon.classList.remove('active'));

    // Ativar seta correspondente
    const currentHeader = elements.table.querySelector(`thead th:nth-child(${columnIndex + 1}) .sort-arrows`);
    const arrowIndex = newDirection === 'asc' ? 0 : 1;
    currentHeader.children[arrowIndex].classList.add('active');

    // Ordenar linhas
    rows.sort((a, b) => {
        let aValue = a.cells[columnIndex].textContent.trim();
        let bValue = b.cells[columnIndex].textContent.trim();

        if (dataType === 'number') {
            aValue = parseFloat(aValue.replace(/[^\d,.-]/g, '').replace(',', '.')) || 0;
            bValue = parseFloat(bValue.replace(/[^\d,.-]/g, '').replace(',', '.')) || 0;
            return newDirection === 'asc' ? aValue - bValue : bValue - aValue;
        } else if (dataType === 'status') {
            const statusOrder = { 'Em Promoção': 1, 'Disponível': 2, 'Indisponível': 3 };
            aValue = statusOrder[aValue] || 4;
            bValue = statusOrder[bValue] || 4;
            return newDirection === 'asc' ? aValue - bValue : bValue - aValue;
        } else {
            return newDirection === 'asc'
                ? aValue.localeCompare(bValue, 'pt-BR')
                : bValue.localeCompare(aValue, 'pt-BR');
        }
    });

    // Reorganizar tabela de forma mais eficiente usando DocumentFragment
    const fragment = document.createDocumentFragment();
    rows.forEach(row => fragment.appendChild(row));
    elements.tbody.appendChild(fragment);
}

function filterTable() {
    const filter = elements.searchInput.value.toLowerCase();
    const rows = elements.tbody.querySelectorAll('tr');

    // Usar requestAnimationFrame para melhor performance
    requestAnimationFrame(() => {
        rows.forEach(row => {
            const cells = row.querySelectorAll('td');
            let found = false;

            // Procurar em todas as células (exceto ações)
            for (let i = 0; i < cells.length - 1; i++) {
                if (cells[i].textContent.toLowerCase().includes(filter)) {
                    found = true;
                    break;
                }
            }

            row.style.display = found ? '' : 'none';
        });
    });
}

// Função de debounce para busca em tempo real
function debouncedFilter() {
    clearTimeout(searchTimeout);
    searchTimeout = setTimeout(filterTable, 300); // 300ms de delay
}

// Inicialização quando DOM estiver pronto
document.addEventListener('DOMContentLoaded', function () {
    elements.init();

    // Permitir busca ao pressionar Enter
    elements.searchInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            clearTimeout(searchTimeout);
            filterTable();
        }
    });

    // Busca com debounce
    elements.searchInput.addEventListener('input', debouncedFilter);
});