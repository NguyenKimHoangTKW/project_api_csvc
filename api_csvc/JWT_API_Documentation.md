# JWT Authentication API Documentation

## Tổng quan
API này cung cấp hệ thống xác thực và phân quyền bằng JWT Token. Tất cả các endpoint JWT đều có prefix `/api/v1/jwt-`.

## Cấu hình JWT (Web.config)
```xml
<appSettings>
    <add key="JWTSecretKey" value="your-super-secret-key-that-is-at-least-32-characters-long-for-jwt-security" />
    <add key="JWTIssuer" value="api_csvc" />
    <add key="JWTAudience" value="api_csvc_users" />
    <add key="JWTExpiryHours" value="24" />
</appSettings>
```

## Authentication Methods

### 1. Login với Google (JWT)
**Endpoint:** `POST /api/v1/jwt-login-with-google`

**Request:**
```json
{
    "email": "user@example.com"
}
```

**Response (Success):**
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id_account": 1,
        "name": "Nguyen Van A",
        "email": "user@example.com",
        "id_role": 1
    },
    "Message": "Đăng nhập thành công với JWT.",
    "success": true
}
```

### 2. Login truyền thống (JWT)
**Endpoint:** `POST /api/v1/jwt-dang-nhap`

**Request:**
```json
{
    "username": "admin",
    "password": "password123"
}
```

**Response (Success):**
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id_account": 1,
        "username": "admin",
        "id_role": 2,
        "name": "Admin User",
        "email": "admin@example.com"
    },
    "message": "Đăng nhập thành công",
    "success": true
}
```

## Protected Endpoints (Yêu cầu JWT Token)

### 3. Lấy thông tin profile
**Endpoint:** `GET /api/v1/jwt-profile`
**Authentication:** Required (JWT)

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:**
```json
{
    "user": {
        "id_account": 1,
        "name": "Nguyen Van A",
        "email": "user@example.com",
        "username": "user123",
        "ten_role": "User",
        "id_role": 1
    },
    "success": true
}
```

### 4. Lấy danh sách tài khoản (Admin only)
**Endpoint:** `GET /api/v1/jwt-admin-accounts`
**Authentication:** Required (JWT)
**Authorization:** Role ID = 2 (Admin)

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:**
```json
{
    "accounts": [
        {
            "id_account": 1,
            "name": "User 1",
            "email": "user1@example.com",
            "username": "user1",
            "ten_role": "User",
            "id_role": 1
        },
        {
            "id_account": 2,
            "name": "Admin User",
            "email": "admin@example.com",
            "username": "admin",
            "ten_role": "Admin",
            "id_role": 2
        }
    ],
    "success": true
}
```

### 5. Cập nhật tài khoản (Admin only)
**Endpoint:** `POST /api/v1/jwt-update-account`
**Authentication:** Required (JWT)
**Authorization:** Role ID = 2 (Admin)

**Request:**
```json
{
    "id_account": 1,
    "ten_role": "Admin"
}
```

**Response:**
```json
{
    "message": "Update tài khoản thành công",
    "success": true
}
```

## Token Management

### 6. Refresh Token
**Endpoint:** `POST /api/v1/jwt-refresh-token`
**Authentication:** Required (JWT)

**Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:**
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "message": "Token refreshed successfully",
    "success": true
}
```

### 7. Validate Token
**Endpoint:** `POST /api/v1/jwt-validate-token`

**Request:**
```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response (Valid Token):**
```json
{
    "message": "Token is valid",
    "user": {
        "id_account": 1,
        "username": "user123",
        "email": "user@example.com",
        "id_role": 1
    },
    "success": true
}
```

**Response (Invalid Token):**
```json
{
    "message": "Invalid token",
    "success": false
}
```

## Error Responses

### 401 Unauthorized (No token hoặc token không hợp lệ)
```json
{
    "message": "Authentication required",
    "success": false
}
```

### 403 Forbidden (Không đủ quyền)
```json
{
    "message": "Insufficient permissions",
    "success": false
}
```

## Cách sử dụng JWT trong ứng dụng

### 1. Login và lưu token
```javascript
// Login
const loginResponse = await fetch('/api/v1/jwt-dang-nhap', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify({
        username: 'admin',
        password: 'password123'
    })
});

const loginData = await loginResponse.json();
if (loginData.success) {
    // Lưu token vào localStorage hoặc sessionStorage
    localStorage.setItem('jwt_token', loginData.token);
}
```

### 2. Sử dụng token cho các API calls
```javascript
// Gọi API với token
const token = localStorage.getItem('jwt_token');
const response = await fetch('/api/v1/jwt-profile', {
    method: 'GET',
    headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
    }
});
```

### 3. Xử lý token expired
```javascript
// Kiểm tra và refresh token
if (response.status === 401) {
    // Token expired, thử refresh
    const refreshResponse = await fetch('/api/v1/jwt-refresh-token', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    });
    
    const refreshData = await refreshResponse.json();
    if (refreshData.success) {
        localStorage.setItem('jwt_token', refreshData.token);
        // Retry original request
    } else {
        // Redirect to login
    }
}
```

## Role-based Authorization

### Danh sách Role IDs
- **1**: User (Người dùng thường)
- **2**: Admin (Quản trị viên)

### Cách sử dụng Attributes
```csharp
// Chỉ yêu cầu authentication
[JwtAuthentication]

// Yêu cầu role cụ thể
[JwtAuthentication]
[JwtAuthorize(2)] // Chỉ admin

// Cho phép nhiều roles
[JwtAuthentication]  
[JwtAuthorize(1, 2)] // User hoặc Admin
```

## Backward Compatibility
Tất cả các API session-based cũ vẫn hoạt động bình thường. JWT APIs được thêm mới với prefix `jwt-`.

## Security Best Practices
1. **Đổi secret key** trong production environment
2. **Sử dụng HTTPS** cho tất cả requests
3. **Đặt thời gian hết hạn token hợp lý** (12-24 giờ)
4. **Implement refresh token mechanism** trong client
5. **Validate token ở client-side** trước khi gửi request
6. **Logout clear token** khỏi client storage 