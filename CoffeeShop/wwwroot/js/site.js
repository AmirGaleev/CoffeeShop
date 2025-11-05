// Функция для асинхронного добавления в корзину
async function addToCart(productId, productName) {
    try {
        const response = await fetch('/cart/add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                productId: productId,
                quantity: 1
            })
        });

        if (response.ok) {
            showNotification(`Товар "${productName}" добавлен в корзину!`, 'success');
            updateCartCounter();
        } else {
            showNotification('Ошибка при добавлении товара в корзину', 'error');
        }
    } catch (error) {
        console.error('Error:', error);
        showNotification('Ошибка сети', 'error');
    }
}

// Показ уведомлений
function showNotification(message, type) {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type === 'success' ? 'success' : 'danger'} alert-dismissible fade show`;
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    const container = document.getElementById('notification-container') || createNotificationContainer();
    container.appendChild(notification);
    
    setTimeout(() => {
        notification.remove();
    }, 3000);
}

function createNotificationContainer() {
    const container = document.createElement('div');
    container.id = 'notification-container';
    container.style.position = 'fixed';
    container.style.top = '20px';
    container.style.right = '20px';
    container.style.zIndex = '9999';
    container.style.minWidth = '300px';
    document.body.appendChild(container);
    return container;
}

// Обновление счетчика корзины
function updateCartCounter() {
    const counter = document.getElementById('cart-counter');
    if (counter) {
        const currentCount = parseInt(counter.textContent) || 0;
        counter.textContent = currentCount + 1;
    }
}

// Загрузка категорий через API
async function loadCategories() {
    try {
        const response = await fetch('/catalog/api/categories');
        const categories = await response.json();
        
        const container = document.getElementById('categories-container');
        if (container) {
            container.innerHTML = categories.map(category => 
                `<a href="/catalog/category/${category}" class="list-group-item list-group-item-action">${category}</a>`
            ).join('');
        }
    } catch (error) {
        console.error('Error loading categories:', error);
    }
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    loadCategories();
});