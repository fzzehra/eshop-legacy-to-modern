# Performance Analysis Report

## 1. Objective

This report documents the initial performance measurements of the legacy ASP.NET Web Forms application and the modern Blazor application using k6.

The objective is to establish baseline response-time measurements and identify opportunities for performance improvement during modernization.

## 2. Test Environment

- **Performance testing tool:** k6 v2.2.0
- **Virtual users (VUs):** 5
- **Test duration:** 30 seconds per application
- **Test type:** HTTP GET load test
- **Test script:** `tests/performance/eshop-load.js`
- **Legacy application:** ASP.NET Web Forms, IIS Express
- **Modern application:** ASP.NET Core Blazor, Kestrel
- **Legacy URL:** `http://localhost:50586/`
- **Modern URL:** `http://localhost:5232/`

Both tests were performed locally with the same k6 configuration.

## 3. Performance Test Results

| Metric | Legacy Web Forms | Modern Blazor |
|---|---:|---:|
| Virtual users | 5 | 5 |
| Duration | 30 seconds | 30 seconds |
| Total HTTP requests | 150 | 150 |
| Successful HTTP checks | 100% | 100% |
| HTTP failure rate | 0% | 0% |
| Average response time | 17.59 ms | 3.58 ms |
| 95th percentile response time (p95) | 39.86 ms | 12.92 ms |
| Requests per second | 4.90 | 4.98 |

### 3.1 Legacy Product Details Performance Test

An additional k6 test was performed against the legacy product details page (`/Catalog/Details/1`).

The test used 5 virtual users for 30 seconds and verified both HTTP 200 responses and the presence of the expected product name.

| Metric | Result |
|---|---:|
| Total HTTP requests | 150 |
| Successful checks | 300/300 (100%) |
| HTTP failure rate | 0% |
| Average response time | 13.63 ms |
| Median response time | 10.29 ms |
| 95th percentile (p95) | 43.69 ms |
| Maximum response time | 74.92 ms |
| Requests per second | 4.92 |

All requests completed successfully, and the expected product information was returned.

This result establishes an initial performance baseline for the legacy product details page. A direct comparison with the modern implementation will be performed when the equivalent modern page is available.

**Test script:** `tests/performance/eshop-details.js`

## 4. Results and Observations

All 150 HTTP requests completed successfully for both applications, with no HTTP failures.

The modern application's measured average response time was approximately 4.9 times lower than the legacy application's average response time.

The modern application's p95 response time was also lower.

However, these measurements are preliminary and do not represent an equivalent workload comparison.

The legacy application's home page displays catalog products, while the modern application's tested home page does not perform the same catalog listing operation.

Therefore, the observed performance difference cannot be attributed solely to the modernization of the application.

## 5. Limitations

- The tests were executed locally rather than in a production-like environment.
- The legacy home page and product details page were tested.
- The modern application's home page was tested, but its equivalent product details page has not yet been measured.
- Database-backed create, update, and delete operations were not measured.
- The legacy and modern home pages do not perform equivalent application functionality.
- Each virtual user waits one second between requests, limiting the generated request rate.
- Only one test run per scenario is documented.
- The results do not establish maximum throughput or scalability.

## 6. Next Steps

1. Test equivalent catalog listing pages when the modern implementation is available.
2. Add HTTP scenarios for product details and other comparable GET endpoints.
3. Evaluate CRUD operations using suitable test data and cleanup procedures.
4. Repeat measurements under consistent conditions.
5. Compare response times, error rates, and throughput under multiple load levels.

## 7. Conclusion

The initial k6 performance tests were completed successfully for both the legacy ASP.NET Web Forms application and the modern Blazor application.

Three test scenarios were measured:

- Legacy catalog listing: 17.59 ms average response time.
- Legacy product details: 13.63 ms average response time.
- Modern home page: 3.58 ms average response time.

All scenarios completed 150 HTTP requests without failures.

These measurements provide initial performance baselines. However, the tested legacy and modern pages do not yet represent equivalent workloads.

A reliable comparison of modernization performance improvements will require testing the modern catalog listing and product details pages once they are available.