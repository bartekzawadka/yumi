export class GetRecipesListQuery {
  searchPhrase = '';
  pageIndex = 0;
  pageSize = 10;
  from: Date = undefined;
  to: Date = undefined;
}
