if __name__ == '__main__':
    from polygon_geohasher.polygon_geohasher import polygon_to_geohashes, geohashes_to_polygon
    from shapely import geometry

    polygon = geometry.Polygon([(-39.1795917, 19.432134), (-39.1656847, 19.429034),
                                (-39.1776492, 19.414236), (-39.1795917, 19.432134)])
    inner_geohashes_polygon = geohashes_to_polygon(polygon_to_geohashes(polygon, 7))
    print (inner_geohashes_polygon)
    outer_geohashes_polygon = geohashes_to_polygon(polygon_to_geohashes(polygon, 7, False))