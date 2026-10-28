# Performance Analysis Report

## 1. Objective

This report documents the performance measurements of the legacy ASP.NET Web Forms application and the modern ASP.NET Core Blazor application using k6.

The objective is to establish baseline response-time measurements, compare equivalent catalog operations, and identify opportunities for performance improvement during modernization.

## 2. Test Environment

- **Performance testing tool:** k6 v2.2.0
- **Virtual users (VUs):** 5
- **Test duration:** 30 seconds per scenario
- **Test type:** HTTP GET load test
- **Legacy application:** ASP.NET Web Forms, IIS Express
- **Modern application:** ASP.NET Core Blazor, Kestrel
- **Legacy base URL:** `http://localhost:50586`
- **Modern base URL:** `http://localhost:5232`
- **Database configuration:** Legacy configured with mock data; Modern connected to SQL Server LocalDB with 12 seeded catalog products.

### Test Scripts

- `tests/performance/eshop-load.js` — Home page
- `tests/performance/eshop-catalog.js` — Modern catalog listing
- `tests/performance/eshop-details.js` — Product details

All tests were executed locally using the same k6 virtual-user and duration settings.

## 3. Performance Test Results

### 3.1 Initial Home Page Measurements

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

These measurements represent different page functionality and should not be interpreted as a direct performance comparison.

An additional modern home page run recorded an average response time of 7.34 ms and a p95 of 16.52 ms, demonstrating variability between runs.

### 3.2 Catalog Listing Performance

The legacy home page displays the product catalog, while the modern application provides its catalog listing at `/catalog`.

| Metric | Legacy Catalog Listing | Modern Catalog Listing |
|---|---:|---:|
| Total HTTP requests | 150 | 150 |
| Successful HTTP checks | 100% | 100% |
| HTTP failure rate | 0% | 0% |
| Average response time | 17.59 ms | **4.72 ms** |
| 95th percentile (p95) | 39.86 ms | **6.40 ms** |
| Requests per second | 4.90 | 4.97 |

The modern catalog test verified HTTP 200 responses and the presence of the expected product name in the returned HTML.

The modern catalog listing demonstrated approximately 3.7 times lower average response time in these measurements.

However, the legacy application was configured to use mock data, while the modern application retrieved catalog data from SQL Server LocalDB. Therefore, the two workloads are functionally similar but not fully equivalent.

**Modern test script:** `tests/performance/eshop-catalog.js`

### 3.3 Product Details Performance

Both applications were tested using the same product details route:

`/Catalog/Details/1`

The tests verified HTTP 200 responses and the presence of the expected product name, `.NET Bot Black Hoodie`.

| Metric | Legacy Product Details | Modern Product Details |
|---|---:|---:|
| Virtual users | 5 | 5 |
| Duration | 30 seconds | 30 seconds |
| Total HTTP requests | 150 | 150 |
| Successful checks | 300/300 (100%) | 300/300 (100%) |
| HTTP failure rate | 0% | 0% |
| Average response time | 13.63 ms | **3.76 ms** |
| Median response time | 10.29 ms | 3.66 ms |
| 95th percentile (p95) | 43.69 ms | **6.30 ms** |
| Maximum response time | 74.92 ms | 10.55 ms |
| Requests per second | 4.92 | 4.98 |

The modern product details page demonstrated approximately 3.6 times lower average response time and 6.9 times lower p95 response time.

All requests completed successfully, and the expected product information was returned by both applications.

**Test script:** `tests/performance/eshop-details.js`

## 4. Results and Observations

All documented test runs completed 150 HTTP requests without HTTP failures.

The modern catalog listing and product details pages demonstrated lower measured response times than their legacy counterparts.

For catalog listing, the modern application recorded an average response time of 4.72 ms compared with 17.59 ms for the legacy application.

For product details, the modern application recorded an average response time of 3.76 ms compared with 13.63 ms for the legacy application.

These results suggest promising performance characteristics for the modern implementation under the tested conditions.

However, the results cannot establish that modernization alone caused the improvements because the applications differ in runtime, hosting configuration, and data access behavior.

## 5. CPU and Memory Usage Comparison

CPU and memory usage were monitored using Windows Performance Monitor (PerfMon) during the local performance testing process.

The legacy application was monitored through the `iisexpress` process, while the modern application was monitored through the `EShop.Modern` process.

| Metric | Legacy ASP.NET Web Forms | Modern Blazor |
|---|---:|---:|
| Average CPU usage | 1.81% | 0.84% |
| Maximum CPU usage | 15.74% | 7.73% |
| Average private memory | 253.03 MiB | 78.13 MiB |
| Maximum private memory | 253.30 MiB | 80.07 MiB |

The modern application showed approximately 53% lower average CPU usage and 69% lower average private memory consumption in the recorded samples.

However, the monitoring sessions included both idle and load-testing periods, and the applications were tested under different hosting and data-access conditions. Therefore, these measurements should be interpreted as preliminary observations rather than definitive evidence of resource-efficiency improvements.

The CPU measurements represent Windows per-process processor time, while the memory measurements represent private bytes rather than total physical RAM usage.

## 6. Limitations

- Tests were executed locally rather than in a production-like environment.
- Legacy and Modern applications used different hosting environments: IIS Express and Kestrel.
- The legacy application was configured with mock data, while the modern application used SQL Server LocalDB.
- Catalog listing pages are functionally similar but not identical in implementation.
- Each virtual user waited one second between requests, limiting the generated request rate.
- The measured request rate was approximately five requests per second and does not represent maximum throughput.
- Only one run per catalog and product details scenario is documented.
- The tests measured HTTP responses rather than complete browser rendering or Blazor interactive behavior.
- Database-backed create, update, and delete operations were not measured.
- - CPU and memory usage were recorded using Windows Performance Monitor, but the monitoring periods included both idle and load-testing activity.
- Results may vary because of caching, application warm-up, and local system activity.

## 7. Next Steps

1. Repeat each scenario multiple times under controlled conditions.
2. Use equivalent data sources and comparable datasets where possible.
3. Evaluate performance at multiple virtual-user levels.
4. Add browser-level tests for interactive Blazor operations.
5. Evaluate CRUD operations using isolated test data and cleanup procedures.
6. Repeat CPU and memory monitoring with synchronized load-testing windows, and include database performance metrics.
7. Consider production-like benchmarking to assess scalability.

## 8. Conclusion

The k6 performance tests were completed successfully for both the legacy ASP.NET Web Forms application and the modern Blazor application.

The following scenarios were measured:

- Legacy catalog listing: 17.59 ms average response time.
- Modern catalog listing: 4.72 ms average response time.
- Legacy product details: 13.63 ms average response time.
- Modern product details: 3.76 ms average response time.
- Modern home page: 3.58 ms average response time in the initial run and 7.34 ms in an additional run.

All documented runs completed 150 HTTP requests without failures.

Under the tested conditions, the modern catalog listing and product details pages showed lower HTTP response times than the legacy implementation.

These results provide useful initial performance baselines and indicate potential benefits of modernization. Further testing with equivalent workloads, repeated measurements, and production-like environments is necessary before drawing definitive performance conclusions.

CPU and memory usage were also monitored using Windows Performance Monitor. The recorded measurements showed lower average CPU usage (0.84% vs. 1.81%) and lower average private memory consumption (78.13 MiB vs. 253.03 MiB) for the modern application compared with the legacy application.

However, these resource utilization measurements included both idle and load-testing periods. Differences in hosting environments, data sources, and monitoring durations limit direct comparability. Additional synchronized benchmarks are required to confirm whether these differences persist under equivalent workloads.