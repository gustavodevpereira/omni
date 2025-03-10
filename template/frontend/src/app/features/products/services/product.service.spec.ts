import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { ProductService } from './product.service';
import { ProductsApiService } from '../../../core/api/services/products-api.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ProductResponses } from '../../../core/api/models/responses.model';

// Interface estendida para testes com campos adicionais
interface ExtendedProductDetails extends ProductResponses.ProductDetails {
  branchExternalId?: string;
  branchId?: string;
  branchName?: string;
}

describe('ProductService', () => {
  let service: ProductService;
  let productsApiServiceMock: jasmine.SpyObj<ProductsApiService>;
  let notificationServiceMock: jasmine.SpyObj<NotificationService>;

  beforeEach(() => {
    // Create spy objects for dependencies
    productsApiServiceMock = jasmine.createSpyObj('ProductsApiService', ['getProducts', 'getProductById']);
    notificationServiceMock = jasmine.createSpyObj('NotificationService', ['success', 'error']);

    TestBed.configureTestingModule({
      providers: [
        ProductService,
        { provide: ProductsApiService, useValue: productsApiServiceMock },
        { provide: NotificationService, useValue: notificationServiceMock }
      ]
    });

    service = TestBed.inject(ProductService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('mapToDomainProduct', () => {
    it('should map API product to domain product with branch data', () => {
      // Mock API response with branch data
      const apiProductWithBranch: ExtendedProductDetails = {
        id: 'product-123',
        name: 'Test Product',
        price: 99.99,
        description: 'Test description',
        category: 'Test Category',
        sku: 'TEST-123',
        stockQuantity: 10,
        status: 'Active',
        createdAt: '2023-01-01',
        updatedAt: null,
        // Adicionando dados da filial
        branchExternalId: 'branch-external-123',
        branchName: 'Test Branch'
      };

      // Configure mock to return our test product
      productsApiServiceMock.getProductById.and.returnValue(of(apiProductWithBranch));

      // Call the service method
      service.getProductById('product-123').subscribe(product => {
        // Verify branch data was correctly mapped
        expect(product.branchExternalId).toBe('branch-external-123');
        expect(product.branchName).toBe('Test Branch');
      });
    });

    it('should use fallback values when branch data is missing in API response', () => {
      // Mock API response without branch data
      const apiProductWithoutBranch: ProductResponses.ProductDetails = {
        id: 'product-123',
        name: 'Test Product',
        price: 99.99,
        description: 'Test description',
        category: 'Test Category',
        sku: 'TEST-123',
        stockQuantity: 10,
        status: 'Active',
        createdAt: '2023-01-01',
        updatedAt: null
      };

      // Configure mock to return our test product
      productsApiServiceMock.getProductById.and.returnValue(of(apiProductWithoutBranch));

      // Call the service method
      service.getProductById('product-123').subscribe(product => {
        // Verify fallback values were used
        expect(product.branchExternalId).toBe('default-branch-id');
        expect(product.branchName).toBe('Default Branch');
      });
    });

    it('should handle branchId as an alternative to branchExternalId', () => {
      // Mock API response with branchId (but not branchExternalId)
      const apiProductWithBranchId: ExtendedProductDetails = {
        id: 'product-123',
        name: 'Test Product',
        price: 99.99,
        description: 'Test description',
        category: 'Test Category',
        sku: 'TEST-123',
        stockQuantity: 10,
        status: 'Active',
        createdAt: '2023-01-01',
        updatedAt: null,
        // Usando branchId em vez de branchExternalId
        branchId: 'branch-id-123',
        branchName: 'Test Branch'
      };

      // Configure mock to return our test product
      productsApiServiceMock.getProductById.and.returnValue(of(apiProductWithBranchId));

      // Call the service method
      service.getProductById('product-123').subscribe(product => {
        // Verify branchId was used as fallback for branchExternalId
        expect(product.branchExternalId).toBe('branch-id-123');
        expect(product.branchName).toBe('Test Branch');
      });
    });
  });
}); 