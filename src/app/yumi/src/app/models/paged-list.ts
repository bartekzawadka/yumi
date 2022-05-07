export class PagedList<T> {
  pageIndex = 0;
  pageSize = 10;
  totalPages = 0;
  totalCount = 0;
  data: T[] = [];
}
